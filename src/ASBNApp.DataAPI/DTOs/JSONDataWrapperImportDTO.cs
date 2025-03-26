using ASBNApp.Models;

namespace ASBNApp.DataAPI.DTOs;

public class JSONDataWrapperImportDTO
{
    public Settings? Settings { get; set; }
    public List<WorkLocation>? WorkLocationHours { get; set; }
    public List<Entry>? Entries { get; set; }
}
