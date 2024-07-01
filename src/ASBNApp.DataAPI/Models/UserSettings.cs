using System.ComponentModel.DataAnnotations;

namespace ASBNApp.DataAPI.Models
{
    public class UserSettings
    {
        public int Id { get; set; } // the user this belongs to

        public string? Username { get; set; }
        public string? Profession { get; set; }
        public string? LegalRepresentitive { get; set; }
        public string? Company { get; set; }
        public string? School { get; set; }
        public DateTimeOffset ApprenticeshipStartDate { get; set; }
    }
}
