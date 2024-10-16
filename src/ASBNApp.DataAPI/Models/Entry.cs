namespace ASBNApp.DataAPI.Models
{
    /// <summary>
    /// The entry for a single day.
    /// </summary>
    public class Entry
    {
        public int Id { get; set; }
        public User Owner { get; set; }

        // This is removing any time zone related information, as there's no point in saving that.
        private DateTime _date;
        public DateTime Date 
        { 
            get => _date;
            set => _date = value.Date; 
        }

        public string? Note { get; set; }
        public string? Location { get; set; }
        public float? Hours { get; set; }
    }
}
