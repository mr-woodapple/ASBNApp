/// <summary>
/// Interface for retrieving & saving data
/// </summary>

using ASBNApp.Frontend.Model;

interface IASBNDataService
{

	public Task<EntryRowModelWithID> GetDay(DateTime? date);
	public Task<bool> SaveDay(EntryRowModelWithID entry);
	public Task<bool> DeleteDay(int? id);

	public Task<List<EntryRowModelWithID>> GetWeek(DateTime? startDate, DateTime? endDate);
    public Task<bool> SaveEntries(IEnumerable<EntryRowModelWithID> entries);

    public Task<Settings> GetSettings();
    public Task<bool> SaveSettings(Settings settings);

    public Task<List<WorkLocationWithID>> GetWorkLocationHours();
    public Task<bool> SaveWorkLocationHours(List<WorkLocationWithID> workLocationHours);
    public Task<bool> DeleteWorkLocationHours(int? id);
}