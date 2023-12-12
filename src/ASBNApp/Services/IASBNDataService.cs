// Created 30.11.2023
// Interface for retrieving & saving data

using ASBNApp.Model;

interface IASBNDataService
{

    public Task ReadData();

    public void WriteData();

    public IEnumerable<EntryRowModel> GetWeek(int? year, int? week);

    public Task SaveWeek(IEnumerable<EntryRowModel> entries);

    public EntryRowModel GetDay(DateTime? date);

    public Task SaveDay(EntryRowModel entry);

    public Settings? GetSettings();

    public Task SaveSettings(Settings settings);

    public List<WorkLocationHours> GetWorkLocationHours();

    public Task SaveWorkLocationHours(WorkLocationHours workLocationHours);
}