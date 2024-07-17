using System.Text.Json.Serialization;

/// <summary>
/// Models for what a single day entry looks like.
/// </summary>

public class EntryRowModel
{
    [JsonIgnore]
    public int? Id { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public float? Hours { get; set; }
}