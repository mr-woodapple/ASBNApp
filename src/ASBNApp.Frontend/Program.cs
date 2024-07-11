using ASBNApp.Frontend;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KristofferStrube.Blazor.FileSystemAccess;
using ASBNApp.Frontend.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// MudBlazor
builder.Services.AddMudServices();

// ASBNDataService dependency injection
builder.Services.AddScoped<IASBNDataService, ASBNDataService>();

// DateHandler service
builder.Services.AddSingleton<DateHandler>();

// Making the FileSystemAccess package available to everyone
builder.Services.AddFileSystemAccessService();

// Base HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
// Add the font service used when generating .pdf's
builder.Services.AddHttpClient<FontServices>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
// API httpClient
builder.Services.AddHttpClient<IASBNDataService, ASBNDataService>(client => client.BaseAddress = new Uri("https://localhost:7148"));


// Add Logger
builder.Services.AddLogging();


await builder.Build().RunAsync();
