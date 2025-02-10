using System.Text.Json;
using System.Text.Json.Nodes;
using ASBNApp.Frontend.Model;

namespace ASBNApp.Frontend.Helpers
{
    public static class LegacyImportHelper
    {
        /// <summary>
        /// Handle converting the data layout from the legacy ASBN App to the new one by manually mapping content.
        /// </summary>
        /// <param name="content">String in JSON format, containing what should be imported.</param>
        /// <returns>A <see cref="JSONDataWrapper"/> object with the data passed to this component.</returns>
        public static JSONDataWrapper ConvertLegacyImport(string content)
        {
            var root = JsonNode.Parse(content);

            // Map settings
            // Map Settings directly with additional logic for splitting Username
            Settings? settings = null;
            var settingsNode = root["Settings"];

            if (settingsNode != null)
            {
                var username = settingsNode["Username"]?.GetValue<string>();
                string? givenName = null;
                string? surname = null;

                if (!string.IsNullOrEmpty(username))
                {
                    // Find the last space in the username
                    int lastSpaceIndex = username.LastIndexOf(' ');
                    if (lastSpaceIndex > 0)
                    {
                        // Split before the last space
                        givenName = username.Substring(0, lastSpaceIndex);
                        surname = username.Substring(lastSpaceIndex + 1);
                    }
                    else
                    {
                        // If there's no space, treat the entire name as GivenName
                        givenName = username;
                    }
                }

                // Map other fields
                settings = new Settings
                {
                    GivenName = givenName,
                    Surname = surname,
                    Username = username,
                    Profession = settingsNode["Profession"]?.GetValue<string>(),
                    LegalRepresentitive = settingsNode["LegalRepresentitive"]?.GetValue<string>(),
                    Company = settingsNode["Company"]?.GetValue<string>(),
                    School = settingsNode["School"]?.GetValue<string>(),
                    ApprenticeshipStartDate = settingsNode["ApprenticeshipStartDate"]?.GetValue<DateTime?>()
                };
            }

            // Map WorkLocations
            var workLocations = new List<WorkLocationWithID>();
            var workLocationsNode = root["WorkLocationHours"]?.AsObject();
            if (workLocationsNode != null)
            {
                foreach (var locationNode in workLocationsNode)
                {
                    var location = locationNode.Value.Deserialize<WorkLocation>();
                    if (location != null)
                    {
                        workLocations.Add(new WorkLocationWithID
                        {
                            Location = location.Location,
                            Hours = location.Hours
                        });
                    }
                }
            }

            // Map Entries
            var loggedData = new List<EntryRowModelWithID>();
            var loggedDataNode = root["LoggedData"]?.AsObject();
            if (loggedDataNode != null)
            {
                foreach (var yearNode in loggedDataNode) // Iterate over years
                {
                    var year = yearNode.Key; // Example: "2024"
                    var weeksNode = yearNode.Value?.AsObject();
                    if (weeksNode != null)
                    {
                        foreach (var weekNode in weeksNode) // Iterate over weeks
                        {
                            var week = weekNode.Key; // Example: "49"
                            var entriesNode = weekNode.Value?.AsObject();
                            if (entriesNode != null)
                            {
                                foreach (var entryNode in entriesNode) // Iterate over individual entries
                                {
                                    var entry = entryNode.Value?.Deserialize<EntryRowModel>();
                                    if (entry != null)
                                    {
                                        loggedData.Add(new EntryRowModelWithID
                                        {
                                            Note = entry.Note,
                                            Date = entry.Date,
                                            Location = entry.Location,
                                            Hours = entry.Hours
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new JSONDataWrapper
            {
                Settings = settings,
                WorkLocationHours = workLocations,
				Entries = loggedData
            };
        }
    }
}
