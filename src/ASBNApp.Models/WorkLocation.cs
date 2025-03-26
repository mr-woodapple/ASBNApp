using System.Text.Json.Serialization;

namespace ASBNApp.Models;

/// <summary>
/// Holds the name and the hours for a location.
/// </summary>
public class WorkLocation
{
	[JsonIgnore]
	public User? Owner { get; set; }

    public int? Id { get; set; } // Primary Key

    public string? LocationName { get; set; }

    public float? SuggestedHours { get; set; }
}
