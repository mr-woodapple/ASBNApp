// Created 2023-12-04
// Implements logic to load data from a local JSON file (which the user can specify)

// TODO: Not thread safe, if required at all?

public class LocalASBNDataService : IASBNDataService
{
    public string InputData;

    public string ReadData() => InputData;

    // Create private variable to store the objects from our JSON in
    public void WriteData(string dataFromJSON) {
        // TODO: Still required??
        // TODO: Check if other occurances of "ReadData" are still present, needs to be WriteData now!
        InputData = dataFromJSON;
        
        Console.WriteLine(InputData);
    }

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