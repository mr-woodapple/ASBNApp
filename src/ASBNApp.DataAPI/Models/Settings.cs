namespace ASBNApp.DataAPI.Models
{
	// Model used to only pass the requested user settings data back to the frontend.
	public class Settings
	{
		public string? GivenName { get; set; }
		public string? Surname { get; set; }
		public string? Username { get; set; }
		public string? Profession { get; set; }
		public string? LegalRepresentitive { get; set; }
		public string? Company { get; set; }
		public string? School { get; set; }
		public DateTime? ApprenticeshipStartDate { get; set; }
	}
}
