// Created 2023-12-04
// Implements logic to load data from a local JSON file (which the user can specify)

using System.Text.Json;
using ASBNApp.Model;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.JSInterop;

public class LocalASBNDataService : IASBNDataService
{
    // Handling dependency injection in the constructor
    public LocalASBNDataService(IFileHandleProvider fileHandles, DateHandler dateHandler, IFileSystemAccessService fileSystemAccessService)
    {
        this.fileHandles = fileHandles;
        this.dateHandler = dateHandler;
        this.fileSystemAccessService = fileSystemAccessService;
    }

    public JSONDataWrapper? DataReadFromJSON;
    private readonly IFileHandleProvider fileHandles;
    private readonly DateHandler dateHandler;
    private readonly IFileSystemAccessService fileSystemAccessService;

    public JSONDataWrapper GetData() => DataReadFromJSON;

    /// <summary>
    /// Load local file from the available list of file handles, then deserializes
    /// the text into JSON objects
    /// </summary>
    /// <returns>Task</returns>
    /// <exception cref="NullReferenceException">Throw exception if </exception>
    public async Task ReadData()
    {
        var fileHandle = fileHandles.GetFileHandles().Single();
        var file = await fileHandle.GetFileAsync();
        var text = await file.TextAsync();

        // Dynamically load the names of objects and files
        DataReadFromJSON = JsonSerializer.Deserialize<JSONDataWrapper>(text) ??
            throw new NullReferenceException(
                $"The selected file ({await fileHandle.GetNameAsync()}) cannot " +
                $"be casted to an {nameof(JSONDataWrapper)}.");
    }


    /// <summary>
    /// Serializes the DataReadFromJSON object into a string with given options,
    /// saves that to disk
    /// </summary>
    /// <returns>Task</returns>
    public async Task WriteData()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string serializedData = JsonSerializer.Serialize(DataReadFromJSON, options);

        var writeable = await fileHandles.GetFileHandles().Single().CreateWritableAsync();
        await writeable.WriteAsync(serializedData);
        await writeable.CloseAsync();
    }


    /// <summary>
    /// Function to get data from the JSON file for a certain date
    /// </summary>
    /// <param name="date">DateTime to load data for</param>
    /// <returns>An EntryRowModel object, either filled with values or empty</returns>
    /// <exception cref="ArgumentNullException">If no date is present, this is thrown</exception>
    public EntryRowModel GetDay(DateTime? date)
    {
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
            catch (Exception e)
            {
                if (e is NullReferenceException || e is KeyNotFoundException)
                    Console.WriteLine(e.Message +
                        " No data available (an empty EntryRowModel has been returned in the meantime).");
                return new EntryRowModel();
            }
        }
        else
        {
            // if there's no date handed over throw Exception
            throw new ArgumentNullException();
        }
    }


    /// <summary>
    /// Creates EntryRowModel object from the arguments, then handles inserting into
    /// the DataReadFromJSON object, also adds new year / week objects if required
    /// and fills them with 5 empty EntryRowModel entries per default
    /// </summary>
    /// <param name="date">DateTime object holding the date to save the entry for</param>
    /// <param name="location">string with user input</param>
    /// <param name="note">string with user input</param>
    /// <returns>Task</returns>
    public async Task SaveDay(DateTime date, string location, string note)
    {
        // Create new object from variables to be added into the main dictionary
        EntryRowModel entry = new EntryRowModel()
        {
            Date = date,
            Location = location,
            Note = note
        };

        // Prepare getting values for inserting into the main dictionary
        string YearAsString = date.ToString("yyyy");
        string DateAsString = date.ToString("yyyy-MM-dd");
        string WeekNumberAsString = dateHandler.GetWeekOfYear(date).ToString();

        try
        {
            // Overwrite the current entry or create a new one
            DataReadFromJSON.LoggedData[YearAsString][WeekNumberAsString][DateAsString] = entry;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message + " No dict entry for this week/year available, creating missing entries now.");

            if (!DataReadFromJSON.LoggedData.ContainsKey(YearAsString))
            {
                // Add new year to the dictionary
                DataReadFromJSON.LoggedData.Add(YearAsString, new Dictionary<string, Dictionary<string, EntryRowModel>>());
            }

            // Add new week to the dictionary
            DataReadFromJSON.LoggedData[YearAsString].Add(WeekNumberAsString, new Dictionary<string, EntryRowModel>());

            // Add 5 new days
            for (var i = 0; i < 5; i++)
            {
                var firstDateOfWeek = dateHandler.GetFirstDateOfWeek(int.Parse(WeekNumberAsString), date.Year);
                DataReadFromJSON.LoggedData[YearAsString][WeekNumberAsString][firstDateOfWeek.AddDays(i).ToString("yyyy-MM-dd")] = new EntryRowModel()
                {
                    Date = firstDateOfWeek.AddDays(i)
                };
            }

            // Replace the empty data with the entry we are about to save
            DataReadFromJSON.LoggedData[YearAsString][WeekNumberAsString][DateAsString] = entry;
        }

        // Call the function that actually saves data to our file
        WriteData();
    }


    /// <summary>
    /// Gets an Enumerable with the entries for a week
    /// </summary>
    /// <param name="year">int for the year to get data for</param>
    /// <param name="week">int for the week to get data for</param>
    /// <returns>Enumerable containing the 5 EntryRowModels (filled with data / empty)</returns>
    public IEnumerable<EntryRowModel> GetWeek(int? year, int? week)
    {
        var result = new List<EntryRowModel>();
        try
        {
            // Retrieve data for the selected week & year
            var dataDictionary = DataReadFromJSON.LoggedData[year.ToString()][week.ToString()];

            // iterate over dict & store in a list 
            foreach (var entry in dataDictionary) { result.Add(entry.Value); }
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e.Message + " No data loaded, please load JSON file before requesting data.");
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message + " No data present for this week, returned empty EntryRowModels instead.");
        }

        // Add 5 entries if dataDictionary list is empty (required for the UI)
        if (result.Count == 0)
        {
            if (week != null && year != null)
            {
                var FirstDateInWeek = dateHandler.GetFirstDateOfWeek((int)week, (int)year);
                for (int i = 0; i < 5; i++)
                {
                    result.Add(new EntryRowModel()
                    {
                        Date = FirstDateInWeek.AddDays(i),
                        Location = string.Empty,
                        Note = string.Empty
                    });
                }
            }
        }

        // Return the list of entries as enumerable
        return result.AsEnumerable();
    }

    /// <summary>
    /// Adds the IEnumerable argument back into the DataReadFromJSON dict,
    /// create new year / week dict if they're not existing
    /// </summary>
    /// <param name="entries"></param>
    /// <returns></returns>
    public async Task SaveWeek(IEnumerable<EntryRowModel> entries)
    {
        // Prepare getting values for inserting into the main dictionary
        var date = entries.ElementAt(0).Date;
        string YearAsString = date.ToString("yyyy");
        string WeekNumberAsString = dateHandler.GetWeekOfYear(date).ToString();

        try
        {
            // Try updating the entries as they are
            foreach (var entry in entries)
            {
                DataReadFromJSON.LoggedData[YearAsString][WeekNumberAsString][entry.Date.ToString("yyyy-MM-dd")] = entry;
            }
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message + " No dict entries for the selected year / week available, creating missing entries now.");

            if (!DataReadFromJSON.LoggedData.ContainsKey(YearAsString))
            {
                // Add new year to the dictionary
                DataReadFromJSON.LoggedData.Add(YearAsString, new Dictionary<string, Dictionary<string, EntryRowModel>>());
            }

            // Add new week to the dictionary
            DataReadFromJSON.LoggedData[YearAsString].Add(WeekNumberAsString, new Dictionary<string, EntryRowModel>());

            // Add entries from the IEnumerable
            foreach (var entry in entries)
            {
                DataReadFromJSON.LoggedData[YearAsString][WeekNumberAsString][entry.Date.ToString("yyyy-MM-dd")] = entry;
            }
        }

        // Call the function that actually saves data to our file
        await WriteData();
    }

    /// <summary>
    /// Get the "Settings" object from the JSON object
    /// </summary>
    /// <returns>Settings object</returns>
    public Settings? GetSettings()
    {
        return DataReadFromJSON.Settings;
    }

    /// <summary>
    /// Updates the "Settings" object
    /// </summary>
    /// <param name="settings">Settings object</param>
    /// <returns>Task</returns>
    public async Task SaveSettings(Settings settings)
    {
        try
        {
            DataReadFromJSON.Settings = settings;
            WriteData();
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message + " No 'Settings' object available, cannot write settings.");
        }
    }

    /// <summary>
    /// Get WorkLocationHours from storage 
    /// </summary>
    /// <returns>List of WorkLocationHour objects</returns>
    public List<WorkLocationHours> GetWorkLocationHours()
    {
        try
        {
            var workLocationHours = new List<WorkLocationHours>();
            var workLocationHoursDictionary = DataReadFromJSON.WorkLocationHours;
            foreach (var entry in workLocationHoursDictionary)
            {
                workLocationHours.Add(entry.Value);
            }
            return workLocationHours;
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e.Message +
                " No data available, please load a file first (returning an empty list of WorkLocationHours in the meantime).");
            return new List<WorkLocationHours>();
        }

    }

    /// <summary>
    /// Update the WorkLocationHours object
    /// </summary>
    /// <param name="workLocationHours"></param>
    /// <returns></returns>
    public async Task SaveWorkLocationHours(List<WorkLocationHours> workLocationHours)
    {
        try
        {
            // Clear the dict (otherwise deleted entries will persist foever)
            DataReadFromJSON.WorkLocationHours.Clear();
            foreach(var entry in workLocationHours) {
                DataReadFromJSON.WorkLocationHours[entry.Location] = entry;
            }
            WriteData();
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message + " No WorkLocationHours object found, please create one first.");
        }
    }

    // Create an empty JSON file, save that to disk
    public async Task CreateEmptyJSON() {
        var fileHandle = fileHandles.GetFileHandles().Single();
        var writePermissionState = await fileHandle.RequestPermissionAsync(new() { Mode = FileSystemPermissionMode.ReadWrite });

    }
}