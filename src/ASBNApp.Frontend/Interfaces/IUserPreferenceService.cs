namespace ASBNApp.Frontend.Interfaces;

public interface IUserPreferenceService
{
	public Task SaveUserPreferences();
	public Task LoadUserPreferences();
}
