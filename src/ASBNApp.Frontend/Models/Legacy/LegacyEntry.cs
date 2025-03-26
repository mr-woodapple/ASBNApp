namespace ASBNApp.Frontend.Models;

/// <summary>
/// Represents an entry from the legacy ASBN App, to only be used in the importer for legacy files!
/// </summary>
[Obsolete("Only to be used in the importer for legacy files! Use ASBNApp.Models.Entry instead.")]
public class LegacyEntry
{
	public string? Note { get; set; }
	public DateTime Date { get; set; }
	public string? Location { get; set; }
	public float? Hours { get; set; }
}
