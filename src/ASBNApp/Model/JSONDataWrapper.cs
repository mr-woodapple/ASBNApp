// Created 2023-12-06
// Wrapper for the data read from the JSON file, holds the top level objects 
// such as WorkLocationHours, Settings & Data

public class JSONDataWrapper
{
    public Settings? Settings { get; set; }
    public Dictionary<string, WorkLocationHours>? WorkLocationHours { get; set; }
    public Dictionary<string, Dictionary<string, Dictionary<string, LogEntryDay>>> LoggedData { get; set; }
}