namespace ASBNApp.Frontend.Models;

/// <summary>
/// Represents a work location from the legacy ASBN App, to only be used in the importer for legacy files!
/// </summary>
[Obsolete("Only to be used in the importer for legacy files! Use ASBNApp.Models.WorkLocation instead.")]
public class LegacyWorkLocation
{
	public string? Location { get; set; }
	public float? Hours { get; set; }
}
