/// <summary>
/// Models how an entry for a single day looks like
/// </summary>

public class EntryRowModel
{
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public float? Hours { get; set; }
}