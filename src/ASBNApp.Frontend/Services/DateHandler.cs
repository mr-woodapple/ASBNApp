using System.Globalization;


/// <summary>
/// Week handler to get various information about weeks
/// </summary>
public class DateHandler {

    /// <summary>
    /// Function to get the current week number
    /// </summary> 
    /// <returns>Week number as an int</returns>
    public int GetCurrentWeekOfYear() 
    {
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
    /// Calculates the week number for a given date.
    /// </summary>
    /// <param name="date"><see cref="DateTime"/> to calculate the week for.</param>
    /// <returns><see cref="int"/> representing the week of year for the given date</returns>
    public int GetWeekOfYear(DateTime date) 
    {
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
    public string GetFirstDayOfWeekAsString() 
    {
        // (int)Date.DayOfWeek returns from a 0 for Sunday up to a 6 for Saturday -> + Monday makes the start of the week Monday
        var FirstDay = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday));
        
        return FirstDay.Date.ToString("dd.MM.");
    }

    /// <summary>
    /// Returns the last day of the current week
    /// </summary>
    /// <returns>Date with day & month as a string</returns>
    public string GetLastDayOfWeekAsString() 
    {
        // get 7 - DayOfWeek, add result to FirstDayOfWeek
        var LastDay = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);

        return LastDay.Date.ToString("dd.MM.yyyy");
    }

    /// <summary>
    /// Returns the Monday date for a given week & year.
    /// </summary>
    /// <returns> DateTime object for the first day in a week </returns>
    public DateTime GetFirstDateOfWeek(int week, int year)
    {
        CultureInfo ci = CultureInfo.CurrentCulture;

        DateTime januaryFirst = new DateTime(year, 1, 1);
        int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)januaryFirst.DayOfWeek;
        DateTime firstWeekDay = januaryFirst.AddDays(daysOffset);

        int firstWeek = ci.Calendar.GetWeekOfYear(januaryFirst, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
        {
            week -= 1;
        }

        return firstWeekDay.AddDays(week * 7);
    }

    /// <summary>
    /// Returns the Sunday date for a given week & year
    /// </summary>
    /// <returns> DateTime object for the first day in a week </returns>
    public DateTime GetLastDateOfWeek(int week, int year) 
    {
        return GetFirstDateOfWeek(week, year).AddDays(6);
    }

    /// <summary>
    /// Calculates in which year of their apprenticeship a user is.
    /// </summary>
    /// <param name="startDate">User specified DateTime</param>
    /// <returns>Int representing the current year.</returns>
    public int CalculateApprenticeshipYear(DateTime? startDate)
    {
        if (startDate == null) return 0;

		DateTime today = DateTime.Now;
		var diff = today - (DateTime)startDate;

		return (diff.Days / 365) + 1;
    }

    /// <summary>
    /// This method is borrowed from http://stackoverflow.com/a/11155102/284240
    /// 
    /// It's different from the <see cref="GetWeekOfYear(DateTime)"/> in how it handles the week,
    /// as the .NET standard allows for weeks to be split across years, which isn't wanted in this case.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public int GetIso8601WeekOfYear(DateTime time)
    {
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }

        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
}