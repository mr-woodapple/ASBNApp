using ASBNApp.DataAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace ASBNApp.DataAPI.Models
{
    public class SeedWorkLocations
    {
        // implement some base data being added if the database is empty
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ASBNAppContext(
                serviceProvider.GetRequiredService<DbContextOptions<ASBNAppContext>>()))
            {
                if (context == null || context.Entry == null)
                {
                    throw new ArgumentNullException("Context is null.");
                }

                // Check if data is present, if so don't add anything
                if (context.WorkLocation.Any())
                { return; }

                context.WorkLocation.AddRange(
                    new WorkLocation
                    {
                        Hours = 7.6f,
                        Location = "Office 1"
                    },

                    new WorkLocation
                    {
                        Hours = 7.2f,
                        Location = "Office 2"
                    },

                    new WorkLocation
                    {
                        Hours = 7.6f,
                        Location = "Home Office"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
