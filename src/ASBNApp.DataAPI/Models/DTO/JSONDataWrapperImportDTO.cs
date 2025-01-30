namespace ASBNApp.DataAPI.Models;

public class JSONDataWrapperImportDTO
{
    public Settings? Settings { get; set; }
    public List<WorkLocation>? WorkLocationHours { get; set; }
    public List<Entry>? Entries { get; set; }
}
