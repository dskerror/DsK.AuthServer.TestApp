using Blazored.LocalStorage;
using DsK.AuthServer.TestApp.Client.Pages;
using DsK.AuthServer.TestApp.Client.Services;
using DsK.AuthServer.TestApp.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

//Add Authorization Core - To be able to use [CascadingAuthenticationState, AuthorizeRouteView, Authorizing], [AuthorizeView, NotAuthorized, Authorized], @attribute [Authorize]
//builder.Services.AddAuthorizationCore();
//The CustomAuthenticationStateProvider is to be able to use tokens as the mode of authentication.
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<SecurityServiceClient>();

/* ---Manages saving to local storage--- */
builder.Services.AddBlazoredLocalStorage();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DsK.AuthServer.TestApp.Client._Imports).Assembly);

app.Run();
