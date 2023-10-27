public class WorkLocationHours {
    public string? Location { get; set; }
    public double Hours { get; set; }

    // demo data for testing purposes, to be removed later on! 
    public static List<WorkLocationHours> DemoWorkLocationData = new() { 
        new WorkLocationHours {Location = "MMS", Hours = 7.6},
        new WorkLocationHours {Location = "HUB Dresden", Hours = 7.2},
        new WorkLocationHours {Location = "BSZ ET DD", Hours = 7.6},
    };
}