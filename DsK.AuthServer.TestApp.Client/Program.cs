using Blazored.LocalStorage;
using DsK.AuthServer.TestApp.Client;
using DsK.AuthServer.TestApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Add Authorization Core - To be able to use [CascadingAuthenticationState, AuthorizeRouteView, Authorizing], [AuthorizeView, NotAuthorized, Authorized], @attribute [Authorize]
builder.Services.AddAuthorizationCore();
//The CustomAuthenticationStateProvider is to be able to use tokens as the mode of authentication.
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<SecurityServiceClient>();

/* ---Manages saving to local storage--- */
builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
