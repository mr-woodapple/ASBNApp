using ASBNApp.Frontend.Interfaces;

namespace ASBNApp.Frontend.Services;

public class UserPreferenceService : IUserPreferenceService
{
	private const string Key = "selectedDarkLightMode";

	public Task LoadUserPreferences()
	{
		throw new NotImplementedException();
	}

	public Task SaveUserPreferences()
	{
		throw new NotImplementedException();
	}
}
