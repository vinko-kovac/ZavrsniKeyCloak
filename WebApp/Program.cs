using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(options =>
{
    // Store the session to cookies
    options.DefaultScheme = "Cookies";// CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";// OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        // URL of the Keycloak server
        options.Authority = "http://localhost:8080/auth/realms/master/";

        // For testing we disable https (should be true for production)
        options.RequireHttpsMetadata = false;
        options.SaveTokens = true;
        options.ClientId = "demo";
        // Client secret shared with Keycloak
        options.ClientSecret = "RUjHbLckD6al2njis5YAaw1vQBlXGK2k";
        //options.ClientSecret = "8YGSFSajRK2SFReCxcl6nSVlslGQFT74";
        options.GetClaimsFromUserInfoEndpoint = true;

        // OpenID flow to use
        options.ResponseType = "code";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
