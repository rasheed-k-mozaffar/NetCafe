﻿using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
#region ConnectionString Construction
var connectionStringBuilder = new SqlConnectionStringBuilder();
connectionStringBuilder.DataSource = "localhost";
connectionStringBuilder.InitialCatalog = "NetCafeDB";
connectionStringBuilder.UserID = builder.Configuration["AppSettings:UserId"];
connectionStringBuilder.Password = builder.Configuration["AppSettings:UserPassword"];
connectionStringBuilder.TrustServerCertificate = true;
#endregion
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register the Application DB Context to the DI Container for future uses.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionStringBuilder.ToString());
});

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

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

