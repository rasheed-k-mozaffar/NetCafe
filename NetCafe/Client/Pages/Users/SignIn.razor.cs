using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace NetCafe.Client.Pages.Users;

public partial class SignIn : ComponentBase
{
    [Inject]
    public IAuthService AuthService { get; set; } = default!;
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;
    [Inject]
    public ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject]
    public AuthenticationStateProvider AuthState { get; set; } = default!;

    private SignInRequest model = new();

    private bool isMakingRequest = false;
    private string? errorMessage = string.Empty;

    private async Task HandleUserSignInAsync()
    {
        isMakingRequest = true;
        errorMessage = string.Empty;

        try
        {
            var result = await AuthService.SignInUserAsync(request: model);

            if (result.IsSuccess)
            {
                // the response includes an access token, set it in the local storage.
                string token = result.Value!;
                await LocalStorage.SetItemAsStringAsync("access_token", token);

                await AuthState.GetAuthenticationStateAsync();
                Navigation.NavigateTo("/");
            }
            else
            {
                errorMessage = "Something went wrong, please try again!";
            }
        }
        catch (AuthenticationFailedException ex)
        {
            errorMessage = ex.Message;
        }

        isMakingRequest = false;
    }
}
