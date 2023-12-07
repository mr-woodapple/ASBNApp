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
    public void WriteData(string dataFromJSON) {
        // convert into actual objects here (JSON deserialization)
        DataReadFromJSON = JsonSerializer.Deserialize<JSONDataWrapper>(dataFromJSON);
        
        // How to access this mess
        // Console.WriteLine(DataReadFromJSON.LoggedData["2023"]["49"]["2023-12-06"].Note);
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

        if (date.HasValue) {
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
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message + " Created an empty RowModelEntry() instead.");
                return new EntryRowModel();
            }
            
        } else {
            // if there's no date handed over throw Exception
            throw new ArgumentNullException();
        }
    }

    public Task<IEnumerable<EntryRowModel>> GetWeek(int? year, int? week)
    {
        // get data for both the 
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