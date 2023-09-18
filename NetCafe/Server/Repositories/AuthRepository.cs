using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NetCafe.Server.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly UserManager<AppUser> userManager;
    private readonly UserIdentityOptions options;
    private readonly ApplicationDbContext context;
    private readonly IConfiguration configuration;

    public AuthRepository(UserManager<AppUser> userManager,
        UserIdentityOptions options,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this.userManager = userManager;
        this.options = options;
        this.context = context;
        this.configuration = configuration;
    }


    public async Task<UserManagerResponse> RegisterUserAsync(RegisterRequest request)
    {
        // check if a user already exists with the same email
        var user = await userManager.FindByEmailAsync(request.Email!);

        if (user is not null)
        {
            return new UserManagerResponse
            {
                Message = "A user already exists with this email",
                HasSucceeded = false
            };
        }

        AppUser userToRegister = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            ProfilePicture = request.ProfilePicture
        };

        var registerationResult = await userManager.CreateAsync(userToRegister, request.Password!);

        // in case the user was registered in the database.
        if (registerationResult.Succeeded)
        {
            // add the user to the standard users role
            var result = await userManager.AddToRoleAsync(userToRegister, "USER");

            if (result.Succeeded) // the user was added successfully to the role
            {
                // check if the user wants to subscribe to the newsletter
                if (request.SubscribeToNewsletter) // add the user to the subscribers
                {
                    var subscriber = new NewsLetterSub
                    {
                        Email = request.Email
                    };

                    // add the email to newsletter subs table
                    await context.NewsLetterSubs.AddAsync(subscriber);
                    await context.SaveChangesAsync();
                }

                return new UserManagerResponse
                {
                    Message = "User registered successfully",
                    HasSucceeded = true
                };
            }
            else
            {
                return new UserManagerResponse
                {
                    Message = "Something has gone wrong while completing the registration",
                    HasSucceeded = false
                };
            }
        }
        else // the user was not registered in the database.
        {
            return new UserManagerResponse
            {
                Message = "Something went wrong, please try again",
                HasSucceeded = false
            };
        }
    }

    public async Task<UserManagerResponse> SignInUserAsync(SignInRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);

        // entered email doesn't exist in the database
        if (user is null)
        {
            return new UserManagerResponse
            {
                Message = "The provided email is either wrong, or doesn't exist in the system",
                HasSucceeded = false
            };
        }

        var passwordMatches = await userManager.CheckPasswordAsync(user, request.Password!);

        if (passwordMatches) // the password is for provided email
        {
            // create the token for the user with the required claims
            // and return it as a string to send it in the response
            var token = await CreateTokenAsync(owner: user);

            return new UserManagerResponse
            {
                Message = token,
                HasSucceeded = true
            };
        }
        else
        {
            return new UserManagerResponse
            {
                Message = "The email or password you entered is incorrect",
                HasSucceeded = false
            };
        }
    }

    private async Task<string> CreateTokenAsync(AppUser owner)
    {
        string SECRET = $"{configuration["JwtSettings:Secret"]}";
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET));

        // adding the required claims for the token
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, owner.Id),
            new Claim(ClaimTypes.Email, owner.Email!),
            new Claim(ClaimTypes.Name, owner.FullName!),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddDays(30).ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, configuration["JwtSettings:Iss"]!),
            new Claim(JwtRegisteredClaimNames.Aud, configuration["JwtSettings:Aud"]!),
        };

        if (await userManager.IsInRoleAsync(owner, "AUTHOR")) // the author of the blog
        {
            claims.Add(new Claim(ClaimTypes.Role, "AUTHOR"));
        }
        else
        {
            // the owner of the token is a standard user.
            claims.Add(new Claim(ClaimTypes.Role, "USER"));
        }

        // building the token
        var token = new JwtSecurityToken
        (
            issuer: configuration["JwtSettings:Iss"],
            audience: configuration["JwtSettings:Aud"],
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.AddDays(30),
            claims: claims
        );

        return tokenHandler.WriteToken(token);
    }
}
