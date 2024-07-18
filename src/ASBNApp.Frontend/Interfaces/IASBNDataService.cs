/// <summary>
/// Interface for retrieving & saving data
/// </summary>

using ASBNApp.Frontend.Model;

interface IASBNDataService
{
    public Task<List<EntryRowModelWithID>> GetWeek(DateTime? startDate, DateTime? endDate);

    public Task SaveWeek(IEnumerable<EntryRowModelWithID> entries);

    public Task<EntryRowModelWithID> GetDay(DateTime? date);

    public Task<bool> SaveDay(EntryRowModelWithID entry);

    public Settings? GetSettings();

    public Task<bool> SaveSettings(Settings settings);

    public Task<List<WorkLocationHours>> GetWorkLocationHours();

    public Task<bool> SaveWorkLocationHours(List<WorkLocationHours> workLocationHours);
}