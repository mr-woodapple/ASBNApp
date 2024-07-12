/// <summary>
/// Models what the settings are supposed to look like
/// </summary>

namespace ASBNApp.Frontend.Model;

public class Settings {
    public string? Username { get; set; }
    public string? Profession { get; set; }
    public string? LegalRepresentitive { get; set; }
    public string? Company { get; set; }
    public string? School { get; set; }
    public DateTime ApprenticeshipStartDate { get; set; }
}