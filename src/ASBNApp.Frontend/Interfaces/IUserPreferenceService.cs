using ASBNApp.Frontend.Enums;

namespace ASBNApp.Frontend.Interfaces;

public interface IUserPreferenceService
{
	public Task SaveUserPreferences(DarkLightMode darkLightMode);
	public Task<DarkLightMode> LoadUserPreferences();
}
