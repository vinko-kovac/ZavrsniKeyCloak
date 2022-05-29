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
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie("Cookies")
    .AddOpenIdConnect(options =>
    {
        // URL of the Keycloak server
        options.Authority = "http://localhost:8080/realms/myrealm/";

        // For testing we disable https (should be true for production)
        options.RequireHttpsMetadata = false;
        options.SaveTokens = true;
        options.ClientId = "demo";
        // Client secret shared with Keycloak
        options.ClientSecret = "FJzKbnpYPBbaUG8AgHpt1e30momnPVF6";
        options.GetClaimsFromUserInfoEndpoint = true;

        // OpenID flow to use
        options.ResponseType = OpenIdConnectResponseType.IdToken;
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
