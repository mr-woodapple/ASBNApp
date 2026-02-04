using ASBNApp.DataAPI.Context;
using ASBNApp.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
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
builder.Services.AddApplicationInsightsTelemetry();

// Configure cookie policy
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent over HTTPS
    options.Cookie.Path = "/";
});

// Configuring CORS
const string frontendCORS = "AllowASBNAppFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontendCORS, policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue<string>("FrontendUrl"))
			.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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

// Set CORS policy
app.UseCors("AllowASBNAppFrontend");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UsePathBase(new PathString("/api"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<User>();
app.MapControllers();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
	Console.WriteLine("Applying migrations...");
	Console.WriteLine($"Connection string used: {builder.Configuration.GetConnectionString("DatabaseConnection")}");
	var db = scope.ServiceProvider.GetRequiredService<ASBNAppContext>();
	db.Database.Migrate();
}

app.Run();