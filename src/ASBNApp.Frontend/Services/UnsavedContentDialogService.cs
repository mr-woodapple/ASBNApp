using ASBNApp.Frontend.Interfaces;

namespace ASBNApp.Frontend.Services;

/// <inheritdoc />
public class UnsavedContentDialogService : IUnsavedContentDialogService
{
	public bool IsDayViewDataSaved { get; set; } = true;
	public bool IsWeekViewDataSaved { get; set; } = true;

	public void ResetDayView() => IsDayViewDataSaved = true;
	public void ResetWeekView() => IsWeekViewDataSaved = true;
}
