using ASBNApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KristofferStrube.Blazor.FileSystemAccess;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
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

// Logger
builder.Services.AddLogging();


await builder.Build().RunAsync();
