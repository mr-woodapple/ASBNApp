namespace ASBNApp.Models;

public class JSONDataWrapper
{
    // TODO: Clarify if this ID is really required.... Or can it be removed?
    public int? ID { get; set; }

    public Settings? Settings { get; set; }

    public List<WorkLocation>? WorkLocationHours { get; set; }

    public List<Entry>? Entries { get; set; }
}