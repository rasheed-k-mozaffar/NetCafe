using Microsoft.AspNetCore.Mvc;

namespace NetCafe.Server.Controllers;

public class AuthController : BaseController
{
    #region Injected Dependencies
    private readonly IAuthRepository authRepository;
    private readonly ILogger<AuthController> logger;
    public AuthController
    (
        IAuthRepository authRepository,
        ILogger<AuthController> logger,
        ApplicationDbContext context
    ) : base(context)
    {
        this.authRepository = authRepository;
        this.logger = logger;
    }
    #endregion

    #region POST Endpoints
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        if (ModelState.IsValid) // the request passed the validations
        {
            // process the sign in request
            var result = await authRepository.SignInUserAsync(request);

            // the sign in was successful, and the access token is generated
            if (result.HasSucceeded)
            {
                logger.LogInformation("Successful sign in for {userName}", request.Email);
                return Ok(new ApiResponse<string>
                {
                    Message = "You've been signed in successfully.",
                    Value = result.Message, // access token
                    IsSuccess = true
                });
            }
            else
            {
                logger.LogError("Failed sign in attempt for {userName}", request.Email);
                // the user failed to sign in due to an incorrect email/ password
                return BadRequest(new ApiErrorResponse
                {
                    Message = result.Message // error message
                });
            }
        }
        else
        {
            logger.LogError("Failed sign in due to invalid data format");
            // entered data doesn't comply with the restrictions
            return BadRequest(new ApiErrorResponse
            {
                Message = "The credentials you entered are invalid.",
                Errors = ModelState.Values.SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList()
            });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (ModelState.IsValid)
        {
            // process the registration
            var result = await authRepository.RegisterUserAsync(request);

            if (result.HasSucceeded)
            {
                logger.LogInformation("New user {fullName} has completed a successful registration with the email {userName}",
                    request.FullName, request.Email);

                return Ok(new ApiResponse
                {
                    Message = result.Message,
                    IsSuccess = true
                });
            }
            else
            {
                logger.LogInformation("Failed registration for {userName}", request.Email);
                return Ok(new ApiErrorResponse
                {
                    Message = result.Message
                });
            }
        }
        else
        {
            logger.LogError("Failed registration due to invalid data format");
            return BadRequest(new ApiErrorResponse
            {
                Message = "The credentials you entered are invalid.",
                Errors = ModelState.Values.SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList()
            });
        }
    }
    #endregion
}




