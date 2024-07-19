using Microsoft.AspNetCore.Identity;

namespace ASBNApp.DataAPI.Models
{
    public class User : IdentityUser
    {
        public string? Profession { get; set; }
        public string? LegalRepresentitive { get; set; }
        public string? Company { get; set; }
        public string? School { get; set; }
        public DateTimeOffset ApprenticeshipStartDate { get; set; }
    }
}
