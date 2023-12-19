// Week handler to get various information about weeks
// Created 17.10.2023

// TODO: Add a date as a parameter to these functions, so they 
// return a value corresponding to the given date


using System.Globalization;

public class DateHandler {

    /// <summary>
    /// Function to get the current week number
    /// </summary> 
    /// <returns>Week number as an int</returns>
    public int GetCurrentWeekOfYear() {
        // get culture info & current date/time from thread
        var CultureInfo = Thread.CurrentThread.CurrentCulture;
        DateTime Date = DateTime.Now;

        DayOfWeek FirstDay = CultureInfo.DateTimeFormat.FirstDayOfWeek;
        CalendarWeekRule weekRule = CultureInfo.DateTimeFormat.CalendarWeekRule;
        Calendar Cal = CultureInfo.Calendar;
        int Week = Cal.GetWeekOfYear(Date, weekRule, FirstDay);

        return Week;
    }

    /// <summary>
    /// Calculates the week number for a given date
    /// </summary>
    /// <returns>int representing the week of year for the given date</returns>
    public int GetWeekOfYear(DateTime date) {
        // Get calendar for current culture info
        var cultureInfo = Thread.CurrentThread.CurrentCulture;
        Calendar calendar = cultureInfo.Calendar;

        // Get the required properties
        CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
        DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

        // Call GetWeekOfYear and return the int value
        return calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);
    }

    /// <summary>
    /// Returns the first day of the current week
    /// </summary>
    /// <returns>Day with day & month as a string</returns>
    public string GetFirstDayOfWeekAsString() {
        // (int)Date.DayOfWeek returns from a 0 for Sunday up to a 6 for Saturday -> + Monday makes the start of the week Monday
        var FirstDay = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday));
        
        return FirstDay.Date.ToString("dd.MM.");
    }

    /// <summary>
    /// Returns the last day of the current week
    /// </summary>
    /// <returns>Date with day & month as a string</returns>
    public string GetLastDayOfWeekAsString() {
        // get 7 - DayOfWeek, add result to FirstDayOfWeek
        var LastDay = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);

        return LastDay.Date.ToString("dd.MM.yyyy");
    }

    /// <summary>
    /// Returns the Monday date for a given week & year
    /// </summary>
    /// <returns> DateTime object for the first day in a week </returns>
    public DateTime GetFirstDateOfWeek(int week, int year) {
        // Get first Monday of a year
        DateTime FirstDayOfAYear = new DateTime(year, 1, 1);
        DateTime FirstMondayOfAYear = new DateTime(year, 1, (8 - (int)FirstDayOfAYear.DayOfWeek) % 7 + 1);

        // Calculate how many days we're in from that first Monday by the number of weeks
        return FirstMondayOfAYear.AddDays((week - 1) * 7);
    }

    /// <summary>
    /// Returns the Sunday date for a given week & year
    /// </summary>
    /// <returns> DateTime object for the first day in a week </returns>
    public DateTime GetLastDateOfWeek(int week, int year) {
        // Get first Monday of a year
        DateTime FirstDayOfAYear = new DateTime(year, 1, 1);
        DateTime FirstMondayOfAYear = new DateTime(year, 1, (8 - (int)FirstDayOfAYear.DayOfWeek) % 7 + 1);

        // Calculate how many days we're in from that first Monday by the number of weeks, 
        // add 6 days to get the last date in a week
        return FirstMondayOfAYear.AddDays((week - 1) * 7 + 6);
    }
}