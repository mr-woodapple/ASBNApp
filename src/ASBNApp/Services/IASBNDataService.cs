// Created 30.11.2023
// Interface for retrieving & saving data

using ASBNApp.Model;

interface IASBNDataService
{

    public JSONDataWrapper ReadData();

    public void WriteData(string text);

    // Retrieving / storing weekly data
    public Task<IEnumerable<EntryRowModel>> GetWeek(int? year, int? week);

    public Task SaveWeek(IEnumerable<EntryRowModel> entries);

    // Retrieving / storing daily data
    public EntryRowModel GetDay(DateTime? date);

    public Task SaveDay(EntryRowModel entry);

    public Settings? GetSettings();

    public Task SaveSettings(Settings settings);

    public List<WorkLocationHours> GetWorkLocationHours();

    public Task SaveWorkLocationHours(WorkLocationHours workLocationHours);
}