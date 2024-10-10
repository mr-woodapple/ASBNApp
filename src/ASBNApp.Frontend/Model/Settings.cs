/// <summary>
/// Model for what to expect from the API when asking for the users settings.
/// </summary>

namespace ASBNApp.Frontend.Model;

public class Settings {
	public string? GivenName { get; set; }
	public string? Surname { get; set; }
	public string? Username { get; set; }
	public string? Profession { get; set; }
	public string? LegalRepresentitive { get; set; }
	public string? Company { get; set; }
	public string? School { get; set; }
	public DateTime? ApprenticeshipStartDate { get; set; }
}