namespace ASBNApp.Frontend.Model
{
    /// <summary>
    /// Basic entry model for what a single day entry looks like.
    /// Without the ID so it can be send to the database.
    /// </summary>
    public class EntryRowModel
    {
        public string? Note { get; set; }
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public float? Hours { get; set; }
    }
}