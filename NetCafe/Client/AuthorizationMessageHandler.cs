
namespace NetCafe.Client;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService localStorage;

    public AuthorizationMessageHandler(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // check to see if the local storage of the browser contains an access token
        if (await localStorage.ContainKeyAsync("access_token"))
        {
            // there's an access token, attach it to the request's header

            // get the token from the local storage
            string token = await localStorage.GetItemAsStringAsync("access_token");
            // add the token to the authorization header with the authentication scheme set to "Bearer"
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
