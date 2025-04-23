namespace ASBNApp.Frontend.Interfaces;

/// <summary>
/// This service allows subpages to check if content has been edited, but not saved.
/// Mainly used on <see cref="Components.App.Views.MainViewDay"/> and <see cref="Components.App.Views.MainViewWeek"/>.
/// </summary>
public interface IUnsavedContentDialogService
{
	bool IsDayViewDataSaved { get; set; }
	bool IsWeekViewDataSaved { get; set; }

	void ResetDayView();
	void ResetWeekView();
}
