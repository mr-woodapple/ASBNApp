using ASBNApp.Frontend;
using ASBNApp.Frontend.Interfaces;
using ASBNApp.Frontend.Services;
using ASBNApp.Frontend.Services.Identity;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using KristofferStrube.Blazor.FileSystemAccess;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Loading the appsettings.json
using (var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
{
	var responseStream = await httpClient.GetStreamAsync("appsettings.json");
	builder.Configuration.AddJsonStream(responseStream);
}

// MudBlazor
builder.Services.AddMudServices();

// Adding dependcy injection services
builder.Services.AddScoped<IASBNDataService, ASBNDataService>();
builder.Services.AddScoped<FontServices>();
builder.Services.AddSingleton<DateHandler>();

// Making the FileSystemAccess package available to everyone
builder.Services.AddFileSystemAccessService();


// Identity related stuff:
// Register the cookie handler
builder.Services.AddTransient<CookieHandler>();

// Set up authorization
builder.Services.AddAuthorizationCore();

// Register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAccountManagement>();

// Register the account management interface
builder.Services.AddScoped(
	sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// Configure client for auth/backend interactions
builder.Services.AddHttpClient(
	"BackendClient",
	client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]))
	.AddHttpMessageHandler<CookieHandler>();

// Configure frontend client
builder.Services.AddHttpClient(
	"FrontendClient",
	client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Add Logger
builder.Services.AddLogging();

await builder.Build().RunAsync();
