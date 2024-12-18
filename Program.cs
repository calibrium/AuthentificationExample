using Microsoft.AspNetCore.Authentication.Cookies;
using AuthentificationExample.Server.Abstractions;
using AuthentificationExample.Server.Exceptions;
using AuthentificationExample.Server.Middleware;
using AuthentificationExample.Server.Services;

var builder = WebApplication.CreateBuilder(args);
    
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddNpgsqlDataSource(connectionString);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<IdentityService>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.Name = "access_token";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseExceptionHandler("/Error");
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.Run();
