namespace NetCafe.Server.Repositories;

public interface IAuthRepository
{
    Task<UserManagerResponse> RegisterUserAsync(RegisterRequest request);
    Task<UserManagerResponse> SignInUserAsync(SignInRequest request);
}
