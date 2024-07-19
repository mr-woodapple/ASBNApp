using Microsoft.EntityFrameworkCore;
using ASBNApp.DataAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ASBNApp.DataAPI.Context
{
    public class ASBNAppContext : IdentityDbContext<IdentityUser>
    {
        public ASBNAppContext(DbContextOptions<ASBNAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Entry> Entry { get; set; } = default!;
        public DbSet<WorkLocation> WorkLocation { get; set; } = default!;
    }
}
