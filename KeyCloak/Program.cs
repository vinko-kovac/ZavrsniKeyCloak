using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;

using Microsoft.Owin.Security;

using Microsoft.Owin.Security.Cookies;

using Owin;

using Owin.Security.Keycloak;

using System;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(options =>
{
    // Store the session to cookies
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationType;
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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


