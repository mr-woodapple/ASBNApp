/// <summary>
/// Models what a combination of work location and hours looks liks
/// </summary>

namespace ASBNApp.Frontend.Model;

public class WorkLocationHours
{
    public int? Id { get; set; }
    public string? Location { get; set; }
    public float? Hours { get; set; }
}