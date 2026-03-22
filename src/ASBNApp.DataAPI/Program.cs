using ASBNApp.DataAPI.Context;
using ASBNApp.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Identity stuff
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ASBNAppContext>()
    .AddApiEndpoints();

// Add various services (Swagger & Application Insights)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASBN App Data API",
        Description = "An ASP.NET Core Web API for handling everything between frontend requests and the database.",
    });

    // get the generated api documentation file, allowing to add comments to the swagger ui
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddEndpointsApiExplorer();

// Configure cookie policy
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent over HTTPS
    options.Cookie.Path = "/";
});

// Configuring CORS (only for local development)
#if DEBUG
Console.WriteLine("Allowed CORS origin: https://localhost:5227");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowASBNAppFrontend", policy =>
    {
        // Local fallback when no environment value is provided.
        policy.WithOrigins("https://localhost:5227")
			.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endif

// Log http details (headers)
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
});

// Create the EDM models
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<User>("User");
modelBuilder.EntitySet<Entry>("Entry");
modelBuilder.EntitySet<WorkLocation>("WorkLocation");

builder.Services.AddControllers().AddOData(
        options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
                "odata",
                modelBuilder.GetEdmModel()));

builder.Services.AddDbContext<ASBNAppContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

// Finalizing
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase(new PathString("/api"));
app.UseCors("AllowASBNAppFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<User>();
app.MapControllers();

// Apply migrations automatically on startup
// Refined logic to handle the "Already Exists" race condition
for (int i = 0; i < 10; i++)
{
	try
	{
		using var scope = app.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<ASBNAppContext>();

		Console.WriteLine("Applying migrations...");
		db.Database.Migrate();

		Console.WriteLine("Migration successfully applied.");
		break;
	}
	catch (SqlException ex) when (ex.Number == 1801) // 1801 = Database already exists
	{
		Console.WriteLine("DB already exists but isn't ready yet. Retrying...");
		Thread.Sleep(2000);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Migration failed: {ex.Message}. Retrying in 5s...");
		Thread.Sleep(5000);
	}
}

app.Run();
