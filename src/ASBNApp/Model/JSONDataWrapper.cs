/// <summary>
/// Wrapper for the data read from the JSON file, holds the top level objects 
/// such as WorkLocationHours, Settings & LoggedData
/// </summary>

using ASBNApp.Model;

public class JSONDataWrapper
{
    public Settings? Settings { get; set; }
    public Dictionary<string, WorkLocationHours>? WorkLocationHours { get; set; }
    public Dictionary<string, Dictionary<string, Dictionary<string, EntryRowModel>>> LoggedData { get; set; }
}