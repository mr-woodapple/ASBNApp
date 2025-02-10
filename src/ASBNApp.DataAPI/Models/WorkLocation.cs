using System.Text.Json.Serialization;

namespace ASBNApp.DataAPI.Models
{
    /// <summary>
    /// Holds the name and the hours for a location.
    /// </summary>
    public class WorkLocation
    {
        [JsonIgnore]
        public int Id { get; set; } // Primary Key
		[JsonIgnore]
		public User Owner { get; set; }
        
        public string? Location { get; set; }
        public float? Hours { get; set; }
    }
}
