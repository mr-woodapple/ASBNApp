/// <summary>
/// Interface for retrieving & saving data
/// </summary>

using ASBNApp.Model;

interface IASBNDataService
{   
    public Task ReadData();

    public Task<bool> WriteData();

    public IEnumerable<EntryRowModel> GetWeek(int? year, int? week);

    public Task<bool> SaveWeek(IEnumerable<EntryRowModel> entries);

    public Task<EntryRowModel> GetDay(DateTime? date);

    public Task<bool> SaveDay(string Note, DateTime Date, string Location, float Hours);

    public Settings? GetSettings();

    public Task<bool> SaveSettings(Settings settings);

    public Task<List<WorkLocationHours>> GetWorkLocationHours();

    public Task<bool> SaveWorkLocationHours(List<WorkLocationHours> workLocationHours);
}