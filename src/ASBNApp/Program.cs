using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KristofferStrube.Blazor.FileSystemAccess;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// ASBNDataService dependency injection
builder.Services.AddSingleton<IASBNDataService, LocalASBNDataService>();
// This registers the FileHandleCollection as a DI service, we then limit
// the available classes to the ones listed here (in this case the same ones the interface implements)
builder.Services.AddSingleton<FileHandleCollection>();
builder.Services.AddSingleton<IFileHandleManager>(s => s.GetRequiredService<FileHandleCollection>());
builder.Services.AddSingleton<IFileHandleProvider>(s => s.GetRequiredService<FileHandleCollection>());
// DateHandler service
builder.Services.AddSingleton<DateHandler>();
// Making the FileSystemAccess package available to everyone
builder.Services.AddFileSystemAccessService();


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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
