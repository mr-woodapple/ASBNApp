using System.ComponentModel.DataAnnotations;

namespace ASBNApp.DataAPI.Models
{
    /// <summary>
    /// The entry for a single day.
    /// </summary>
    public class Entry
    {
        public int Id { get; set; }

        public DateTimeOffset Date { get; set; }
        public string? Note { get; set; }
        public string? Location { get; set; }
        public float? Hours { get; set; }


    }
}
