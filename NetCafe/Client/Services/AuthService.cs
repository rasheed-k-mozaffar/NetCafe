
using System.Net.Http.Json;

namespace NetCafe.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient httpClient;

    public AuthService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task RegisterUserAsync(RegisterRequest request)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/auth/register", request);

        if (!response.IsSuccessStatusCode)
        {
            // if the server returned an error, throw it here and catch it later
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new AuthenticationFailedException(message: error!.Message!);
        }
    }

    public async Task<ApiResponse<string>> SignInUserAsync(SignInRequest request)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/auth/sign-in", request);

        if (!response.IsSuccessStatusCode)
        {
            // if the server returned an error, throw it here and catch it later
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new AuthenticationFailedException(message: error!.Message!);
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return result!;
    }
}
