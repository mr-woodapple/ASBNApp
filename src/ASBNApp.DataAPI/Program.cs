using ASBNApp.Models;
using ASBNApp.DataAPI.Context;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;


var builder = WebApplication.CreateBuilder(args);

// Identity stuff
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();
builder.Services.AddIdentityCore<User>()
	.AddEntityFrameworkStores<ASBNAppContext>()
	.AddApiEndpoints();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add ApplicationInsights logging:
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
modelBuilder.EntitySet<JSONDataWrapper>("Import");

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

// Enable Swagger for local development
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

app.Run();