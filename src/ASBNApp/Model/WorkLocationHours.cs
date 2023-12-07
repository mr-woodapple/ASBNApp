// Created 2023-12-06
// Models what a combination of work location and hours looks liks


namespace ASBNApp.Model;

public class WorkLocationHours
{
    public string? Location { get; set; }
    public float? Hours { get; set; }


    // demo data for testing purposes, to be removed later on! 
    public static List<WorkLocationHours> DemoWorkLocationData = new() {
        new WorkLocationHours { Location = "MMS", Hours = 7.6f },
        new WorkLocationHours { Location = "HUB Dresden", Hours = 6.2f },
        new WorkLocationHours { Location = "BSZ ET DD", Hours = 7.2f },
    };
}