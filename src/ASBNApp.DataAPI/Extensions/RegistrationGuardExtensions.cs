namespace ASBNApp.DataAPI.Extensions;

/// <summary>
/// Allow options regarding registration.
/// </summary>
public static class RegistrationGuardExtensions
{
    /// <summary>
    /// Handling disabled registration by returning a 403 status code.
    /// </summary>
    public static IApplicationBuilder UseRegistrationGuard(this IApplicationBuilder app, IConfiguration configuration)
    {
        var allowRegistration = configuration.GetValue("FeatureFlags:AllowRegistration", true);

        return app.Use(async (context, next) =>
        {
            if (!allowRegistration
                && HttpMethods.IsPost(context.Request.Method)
                && context.Request.Path.Equals("/register", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await next();
        });
    }
}
