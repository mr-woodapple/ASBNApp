using ASBNApp.Models;

namespace ASBNApp.Frontend.Helpers;

/// <summary>
/// Simple helper that will set "DateTimeKind.None" on all DateTime values before converting them to JSON
/// </summary>
public static class DateTimeKindHelper
{
	public static Entry SetDateTimeKindToNone(Entry entry)
	{
		entry.Date = DateTime.SpecifyKind(entry.Date, DateTimeKind.Utc);
		return entry;
	}
}
