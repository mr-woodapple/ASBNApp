using ASBNApp.Models;

namespace ASBNApp.Frontend.Interfaces;

/// <summary>
/// Interface for retrieving & saving data
/// </summary>
interface IASBNDataService
{
	public Task<Entry?> GetDay(DateTime? date);
	public Task<HttpResponseMessage> SaveDay(Entry entry);
	public Task<bool> DeleteDay(int? id);

	public Task<List<Entry>> GetWeek(DateTime? startDate, DateTime? endDate);
    public Task<bool> SaveEntries(IEnumerable<Entry> entries);
    public Task<List<Entry>> GetAllEntries();

    public Task<Settings> GetSettings();
    public Task<bool> SaveSettings(Settings settings);

    public Task<List<WorkLocation>> GetWorkLocationHours();
    public Task<bool> SaveWorkLocationHours(List<WorkLocation> workLocationHours);
    public Task<bool> DeleteWorkLocationHours(int? id);

    public Task<HttpResponseMessage> ImportData(JSONDataWrapper data);
}