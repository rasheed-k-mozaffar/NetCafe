namespace NetCafe.Client.Services;

public interface IAuthService
{
    Task RegisterUserAsync(RegisterRequest request);
    Task<ApiResponse<string>> SignInUserAsync(SignInRequest request);
}
