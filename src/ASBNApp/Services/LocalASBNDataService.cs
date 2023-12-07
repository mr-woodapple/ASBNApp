// Created 2023-12-04
// Implements logic to load data from a local JSON file (which the user can specify)

using System;
using System.Globalization;
using System.Text.Json;

public class LocalASBNDataService : IASBNDataService
{
    public JSONDataWrapper DataReadFromJSON;
    public JSONDataWrapper ReadData() => DataReadFromJSON;


    // Stores the string argument in the InputData variable
    // Convert to actual objects here
    public void WriteData(string dataFromJSON)
    {
        // convert into actual objects here (JSON deserialization)
        DataReadFromJSON = JsonSerializer.Deserialize<JSONDataWrapper>(dataFromJSON);

    }

    /// <summary>
    /// Function to get data from the JSON file for a certain date
    /// </summary>
    /// <param name="date">DateTime to load data for</param>
    /// <returns>An EntryRowModel object, either filled with values or empty</returns>
    /// <exception cref="ArgumentNullException">If no date is present, this is thrown</exception>
    public EntryRowModel GetDay(DateTime? date)
    {
        DateHandler dateHandler = new DateHandler();

        string YearAsString;
        string DateToLoadAsString;
        string WeekNumberAsString;

        if (date.HasValue)
        {
            // Need to run through this because we're using a nullable date
            // Get the parameters we need to get the LoggedData
            YearAsString = date.Value.ToString("yyyy");
            DateToLoadAsString = date.Value.ToString("yyyy-MM-dd");
            WeekNumberAsString = dateHandler.GetWeekOfYear((DateTime)date).ToString();

            // try catch block here to check if we have a key present
            try
            {
                return DataReadFromJSON.LoggedData[YearAsString][WeekNumberAsString][DateToLoadAsString];
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message + 
                    " Specify a data source before trying to access data (an empty EntryRowModel has been returned in the meantime).");
                return new EntryRowModel();
            }
        }
        else
        {
            // if there's no date handed over throw Exception
            throw new ArgumentNullException();
        }
    }

    public async Task<IEnumerable<EntryRowModel>> GetWeek(int? year, int? week)
    {
        // List of entries to return as Enumerable 
        // TODO: Does this make sense at all?
        var result = new List<EntryRowModel>();
        try
        {
            // Retrieve data for the selected week & year
            var dataDictionary = DataReadFromJSON.LoggedData[year.ToString()][week.ToString()];

            // iterate over dict & store in a list 
            foreach (var entry in dataDictionary)
            {
                result.Add(entry.Value);
            }
        }
        catch
        {
            // if an exception was catched, just return an set of 5 empty EntryRowModels
        }


        // return the list of entries as enumerable
        return await Task.FromResult(result.AsEnumerable());
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