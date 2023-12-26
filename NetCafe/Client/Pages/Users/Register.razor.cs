using Microsoft.AspNetCore.Components;

namespace NetCafe.Client.Pages.Users;

public partial class Register : ComponentBase
{
    [Inject]
    public IAuthService AuthService { get; set; } = default!;
    [Inject]
    public NavigationManager Nav { get; set; } = default!;

    private RegisterRequest model = new();
    private bool isMakingRequest = false;
    private string? errorMessage = string.Empty;
    private async Task HandleUserRegistrationAsync()
    {
        // disable the buttons so the user doesn't send multiple requests
        isMakingRequest = true;
        errorMessage = string.Empty;
        try
        {
            await AuthService.RegisterUserAsync(model);
            Nav.NavigateTo("/user/sign-in");
        }
        catch (AuthenticationFailedException ex)
        {
            // store the error message received from the server to display it on the page
            errorMessage = ex.Message;
        }

        isMakingRequest = false;
    }
}
