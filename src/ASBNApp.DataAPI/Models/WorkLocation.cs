using System.ComponentModel.DataAnnotations;

namespace ASBNApp.DataAPI.Models
{
    /// <summary>
    /// Holds the name and the hours for a location.
    /// </summary>
    public class WorkLocation
    {
        public int Id { get; set; } // Primary Key
        public User Owner { get; set; }
        
        public string? Location { get; set; }
        public float? Hours { get; set; }
    }
}
