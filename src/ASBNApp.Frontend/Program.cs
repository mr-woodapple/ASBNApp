using ASBNApp.Frontend;
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

// MudBlazor
builder.Services.AddMudServices();

// Adding dependcy injection services
builder.Services.AddScoped<IASBNDataService, ASBNDataService>();
builder.Services.AddSingleton<DateHandler>();

// Making the FileSystemAccess package available to everyone
builder.Services.AddFileSystemAccessService();

// Adding base HttpClients
// TODO: Simplify these into two clients, one for the base adress and one for the backend!
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<FontServices>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient<IASBNDataService, ASBNDataService>(client => client.BaseAddress = new Uri("https://localhost:7148"));


// Identity related stuff:
// Register the cookie handler
builder.Services.AddTransient<CookieHandler>();

// Set up authorization
builder.Services.AddAuthorizationCore();

// Register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationService>();

// Register the account management interface
builder.Services.AddScoped(
	sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// Configure client for auth interactions
builder.Services.AddHttpClient(
	"Auth",
	client => client.BaseAddress = new Uri("https://localhost:7148"))
	.AddHttpMessageHandler<CookieHandler>();


// Add Logger
builder.Services.AddLogging();

await builder.Build().RunAsync();
