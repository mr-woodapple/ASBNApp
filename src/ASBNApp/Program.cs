using ASBNApp;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KristofferStrube.Blazor.FileSystemAccess;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// MudBlazor
builder.Services.AddMudServices();

// ASBNDataService dependency injection
builder.Services.AddScoped<IASBNDataService, LocalASBNDataService>();
// This registers the FileHandleCollection as a DI service, we then limit
// the available classes to the ones listed here (in this case the same ones the interface implements)
builder.Services.AddSingleton<FileHandleCollection>();

builder.Services.AddSingleton<IFileHandleManager>(s => s.GetRequiredService<FileHandleCollection>());
builder.Services.AddSingleton<IFileHandleProvider>(s => s.GetRequiredService<FileHandleCollection>());

// DateHandler service
builder.Services.AddSingleton<DateHandler>();

// Making the FileSystemAccess package available to everyone
builder.Services.AddFileSystemAccessService();

// Add the font service for exporting PDFs
builder.Services.AddHttpClient<FontServices>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
// Add the http client to load the pdf template
builder.Services.AddHttpClient("httpClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Add Logger
builder.Services.AddLogging();


await builder.Build().RunAsync();
