﻿using ASBNApp.Frontend.Components.Settings;

namespace ASBNApp.Frontend.Model
{
    /// <summary>
    /// Wrapper for the data read from the JSON file, holds the top level objects 
    /// such as WorkLocationHours, Settings & LoggedData
    /// </summary>
    public class JSONDataWrapper
    {
        public Settings? Settings { get; set; }
        public List<WorkLocationWithID>? WorkLocationHours { get; set; }
        public List<EntryRowModelWithID>? Data { get; set; }
    }
}
