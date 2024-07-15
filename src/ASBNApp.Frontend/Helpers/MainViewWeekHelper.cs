namespace ASBNApp.Frontend.Helpers
{
    public static class MainViewWeekHelper
    {
        /// <summary>
        /// Handles logic for creating and sorting entries received from the data source. 
        /// 
        /// Makes sure we have at least 5 entries present, also tries to replace empty days during the week
        /// with content from the weekend.
        /// </summary>
        /// <param name="entries">List of <see cref="EntryRowModel"/>s from the data source.</param>
        /// <param name="selectedWeek">The selected week of the year.</param>
        /// <param name="selectedYear">The selected year.</param>
        /// <returns>A List of <see cref="EntryRowModel"/>s, ready to display.</returns>
        public static List<EntryRowModel> GetCompleteWeek(List<EntryRowModel> entries, int? selectedWeek, int? selectedYear)
        {
            var dateHandler = new DateHandler();
            var possibleDates = new List<DateTime>();
            var startDate = dateHandler.GetFirstDateOfWeek((int)selectedWeek, (int)selectedYear);
            for (int i = 0; i < 7; i++) { possibleDates.Add(startDate.AddDays(i)); }

            // Create dict from possible dates, then fill with the data received
            var dict = new Dictionary<DateTime, EntryRowModel?>();
            dict = possibleDates.ToDictionary(e => e.Date, e => (EntryRowModel)null);

            foreach (var entry in entries)
            {
                if (dict.ContainsKey(entry.Date))
                {
                    dict[entry.Date] = entry;
                }
            }

            // Create a list with the allowed dates
            // Remove the first two entries, as we don't want to change anything to Saturday or Sunday
            var allowedDates = possibleDates;
            allowedDates.Reverse();
            allowedDates.RemoveRange(0, 2);

            // Remove entries for Saturday and Sunday, then sort the Dict
            var saturdayEntry = dict[startDate.AddDays(5)];
            var sundayEntry = dict[startDate.AddDays(6)];

            // Set int based on how many days on the weekend are != null
            // If both != null: 2, just one != null: 1, both = null: 0
            int nullWeekendDays = (saturdayEntry != null && sundayEntry != null) ? 2 : (saturdayEntry != null || sundayEntry != null) ? 1 : 0;

            // For every day on the weekend, that is != null, find an empty date during the week to remove.
            if (nullWeekendDays != 0)
            {
                for (int i = 0; i < nullWeekendDays; i++)
                {
                    foreach (var date in allowedDates)
                    {
                        if (dict[date] == null)
                        {
                            dict.Remove(date);
                            allowedDates.Remove(date);
                            break;
                        }
                    }
                }
            } 
            else
            {
                dict.Remove(startDate.AddDays(5));
                dict.Remove(startDate.AddDays(6));
            }


            // Sort the dict based on the keys
            var sorted = dict.OrderBy(e => e);

            // Create list to return
            var completeEntries = new List<EntryRowModel>();
            foreach(KeyValuePair<DateTime, EntryRowModel?> entry in dict)
            {
                var e = entry.Value == null
                    ? new EntryRowModel { Date = entry.Key }
                    : entry.Value;

                completeEntries.Add(e);
            }

            return completeEntries;
        }
    }
}
