namespace ASBNApp.Frontend.Services;

// TODO: Make this a DI service, so it's not required to be static
public static class UnsavedContentDialogService
{
	public static bool IsDayViewDataSaved { get; set; } = true;
	public static bool IsWeekViewDataSaved { get; set; } = true;

	public static void ResetDayView() => IsDayViewDataSaved = true;
	public static void ResetWeekView() => IsWeekViewDataSaved = true;


}
