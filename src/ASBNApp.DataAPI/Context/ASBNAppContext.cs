using Microsoft.EntityFrameworkCore;
using ASBNApp.DataAPI.Models;

namespace ASBNApp.DataAPI.Context
{
    public class ASBNAppContext : DbContext
    {
        public ASBNAppContext(DbContextOptions<ASBNAppContext> options) : base(options) { }


        public DbSet<Entry> Entry { get; set; } = default!;
        public DbSet<WorkLocation> WorkLocation { get; set; } = default!;
    }
}
