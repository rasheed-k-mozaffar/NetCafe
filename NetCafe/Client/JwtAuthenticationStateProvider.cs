using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace NetCafe.Client;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorage;

    public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await localStorage.ContainKeyAsync("access_token"))
        {
            // the user is logged in
            // get the JWT token from the local storage to read the claims inside it
            string jwt = await localStorage.GetItemAsStringAsync("access_token");
            // read the string token, and converts it into a jwt security token
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

            // create the user's identity (Claims Based Identity)
            ClaimsIdentity identity = new ClaimsIdentity(token.Claims, "Bearer");

            // create the user with all the claims fetched from the token
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            // create the authentication state for the current authenticated user
            AuthenticationState authState = new AuthenticationState(user);
            // raise the event that the authentication state of the user has changed
            // and now they're authenticated
            NotifyAuthenticationStateChanged(Task.FromResult(authState));

            return authState;
        }
        // the user is not logged in
        ClaimsPrincipal anonymousUser = new ClaimsPrincipal();
        AuthenticationState anonymousAuthState = new AuthenticationState(anonymousUser);

        return anonymousAuthState;
    }
}
