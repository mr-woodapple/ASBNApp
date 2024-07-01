using ASBNApp.DataAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace ASBNApp.DataAPI.Models
{
    public class SeedEntries
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
                if (context.Entry.Any())
                { return; }

                context.Entry.AddRange(
                    new Entry
                    {
                        Date = DateTime.Now,
                        Hours = 7.6f,
                        Location = "Office 1",
                        Note = "Testeintrag Montag"
                    },

                    new Entry
                    {
                        Date = DateTime.Now.AddDays(1),
                        Hours = 7.6f,
                        Location = "Office 2",
                        Note = "Testeintrag Dienstag"
                    },

                    new Entry
                    {
                        Date = DateTime.Now.AddDays(2),
                        Hours = 7.6f,
                        Location = "Office 1",
                        Note = "Testeintrag Mittwoch"
                    },

                    new Entry
                    {
                        Date = DateTime.Now.AddDays(3),
                        Hours = 7.6f,
                        Location = "Office 2",
                        Note = "Testeintrag Donnerstag"
                    },

                    new Entry
                    {
                        Date = DateTime.Now.AddDays(4),
                        Hours = 7.6f,
                        Location = "Home Office",
                        Note = "Testeintrag Freitag"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
