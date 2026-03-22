using Microsoft.EntityFrameworkCore;
using ASBNApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ASBNApp.DataAPI.Context;

/// <summary>
/// EF Core database context, including ASP.NET Core Identity for user management.
/// </summary>
public class ASBNAppContext(DbContextOptions<ASBNAppContext> options) : IdentityDbContext<IdentityUser>(options)
{
	/// <summary>
	/// The users of the application.
	/// </summary>
	public DbSet<User> AppUsers { get; set; } = default!;

	/// <summary>
	/// The daily entries.
	/// </summary>
	public DbSet<Entry> LogEntry { get; set; } = default!;

	/// <summary>
	/// The WorkLocation items.
	/// </summary>
	public DbSet<WorkLocation> WorkLocation { get; set; } = default!;
}