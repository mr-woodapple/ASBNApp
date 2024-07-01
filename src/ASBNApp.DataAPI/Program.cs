using ASBNApp.DataAPI.Context;
using ASBNApp.DataAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Setting up the database
var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

builder.Services.AddControllers();
builder.Services.AddDbContext<ASBNAppContext>(
    options => options.UseSqlServer(config.GetConnectionString("Default")));

// Finalizing
var app = builder.Build();


// Seed database if no data is present
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedEntries.Initialize(services);
    SeedWorkLocations.Initialize(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
