// Created 30.11.2023
// Interface for retrieving & saving data

using ASBNApp.Shared.Components.UI;

interface IASBNDataService
{
    // Retrieving / storing weekly data
    public Task<IEnumerable<EntryRowModel>> GetWeek(int? year, int? week);

    public Task SaveWeek(IEnumerable<EntryRowModel> entries);

    // Retrieving / storing daily data
    public Task<EntryRowModel> GetDay(DateTime? date);

    public Task SaveDay(EntryRowModel entry);
}