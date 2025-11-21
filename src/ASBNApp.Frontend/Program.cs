using ASBNApp.Frontend;
using ASBNApp.Frontend.Interfaces;
using ASBNApp.Frontend.Services;
using ASBNApp.Frontend.Services.Identity;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using KristofferStrube.Blazor.FileSystemAccess;
using MudBlazor.Services;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Loading the appsettings.json
using (var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
{
    var response = await httpClient.GetAsync("appsettings.json");
    using var stream = await response.Content.ReadAsStreamAsync();
    builder.Configuration.AddJsonStream(stream);
}

// Adding external (installed) services
builder.Services.AddMudServices();
builder.Services.AddFileSystemAccessService();
builder.Services.AddBlazoredLocalStorageAsSingleton();

// Adding custom services
builder.Services.AddScoped<IASBNDataService, ASBNDataService>();
builder.Services.AddScoped<FontServices>();
builder.Services.AddSingleton<DateHandler>();
builder.Services.AddSingleton<LayoutService>();
builder.Services.AddSingleton<IUserPreferenceService, UserPreferenceService>();
builder.Services.AddSingleton<IUnsavedContentDialogService, UnsavedContentDialogService>();

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