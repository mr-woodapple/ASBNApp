using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASBNApp.Models;

/// <summary>
/// The entry for a single day.
/// </summary>
public class Entry
{
    [JsonIgnore]
    public User? Owner { get; set; }

    public int? Id { get; set; }
	
    // This is removing any time zone related information, as there's no point in saving that.
    private DateTime _date;
    public DateTime Date 
    { 
        get => _date;
        set => _date = value.Date; 
    }

    public string? Note { get; set; }

    [ForeignKey(nameof(LocationId))]
    public WorkLocation? Location { get; set; }

    public int? LocationId { get; set; }

    public float? Hours { get; set; }
}