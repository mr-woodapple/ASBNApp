// Created 2023-12-04
// Implements logic to load data from a local JSON file (which the user can specify)


public class LocalASBNDataService : IASBNDataService
{

    // Create private variable to store the objects from our JSON in


    public Task<EntryRowModel> GetDay(DateTime? date)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<EntryRowModel>> GetWeek(int? year, int? week)
    {
        throw new NotImplementedException();
    }

    public Task SaveDay(EntryRowModel entry)
    {
        throw new NotImplementedException();
    }

    public Task SaveWeek(IEnumerable<EntryRowModel> entries)
    {
        throw new NotImplementedException();
    }
}