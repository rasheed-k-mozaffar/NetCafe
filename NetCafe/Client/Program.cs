using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetCafe.Client;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();

// register the custom-made services here
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<IMailingListService, MailingListService>();

// register the Authorization message handler as a transient service.
builder.Services.AddTransient<AuthorizationMessageHandler>();
// register the Local Storage Service in the DI container.
builder.Services.AddBlazoredLocalStorage();
// add the custom-made authentication system
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
// Create the HTTP Client and configure its base address + Attach the authorization handler to it.
builder.Services.AddHttpClient("NetCafe.ServerAPI", client =>
{
    // set the URL address for the Client to send the requests to.
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project.
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("NetCafe.ServerAPI"));
await builder.Build().RunAsync();

