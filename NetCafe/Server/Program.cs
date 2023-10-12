using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using NetCafe.Server;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Serilog Config
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
#endregion
#region ConnectionString Construction
var connectionStringBuilder = new SqlConnectionStringBuilder();
connectionStringBuilder.DataSource = "localhost";
connectionStringBuilder.InitialCatalog = "NetCafeDB";
connectionStringBuilder.UserID = builder.Configuration["AppSettings:UserId"];
connectionStringBuilder.Password = builder.Configuration["AppSettings:UserPassword"];
connectionStringBuilder.TrustServerCertificate = true;
#endregion
#region Serivces Registration
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSerilog();
// Register the Application DB Context to the DI Container for future uses.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionStringBuilder.ToString());
});

// registering repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<ITagsRepository, TagsRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IMailingListRepository, MailingListRepository>();
#endregion
#region Identity Config
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Configuring the acceptable passwords.
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
}).AddDefaultTokenProviders()
  .AddEntityFrameworkStores<ApplicationDbContext>();
#endregion
#region Bearer Authentication Config
// adding bearer authentication
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});
#endregion
#region User Identity Options
// this service provides access to the authenticated user's ID 
builder.Services.AddScoped(sp =>
{
    var options = new UserIdentityOptions();

    // this line gives us access to the context accessor interface
    // so that we can retrieve the http context and access the current
    // authenticated user
    var httpContextAccessor = sp.GetService<IHttpContextAccessor>();

    if (httpContextAccessor is not null)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is not null && httpContext.User.Identity!.IsAuthenticated)
        {
            options.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            options.UserName = httpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        }
    }
    return options;
});

#endregion
Log.Information("Starting the application at {timeOfStarting}", DateTime.Now.ToShortTimeString());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

