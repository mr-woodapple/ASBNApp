// Created 2023-12-04
// Implements logic to load data from a local JSON file (which the user can specify)

using System;
using System.Text.Json;

public class LocalASBNDataService : IASBNDataService
{
    public string InputData;

    // Returns InputData
    public string ReadData() => InputData;

    // Stores the string argument in the InputData variable
    // Convert to actual objects here
    public void WriteData(string dataFromJSON) {
        InputData = dataFromJSON;

        // convert into actual objects here (JSON deserialization)
        JSONDataWrapper jsonDataWrapper = JsonSerializer.Deserialize<JSONDataWrapper>(dataFromJSON);
        // How to access this mess
        Console.WriteLine(jsonDataWrapper.LoggedData["2023"]["49"]["2023-12-04"].Note);

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