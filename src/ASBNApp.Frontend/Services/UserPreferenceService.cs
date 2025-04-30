using Blazored.LocalStorage;
using ASBNApp.Frontend.Enums;
using ASBNApp.Frontend.Interfaces;

namespace ASBNApp.Frontend.Services;

public class UserPreferenceService : IUserPreferenceService
{
	private const string Key = "selectedDarkLightMode";
	private readonly ILocalStorageService _localStorageService;

	public UserPreferenceService(ILocalStorageService localStorageService)
	{
		_localStorageService = localStorageService;
	}

	/// <summary>
	/// Retrieve the preferred color scheme from local storage (for the hardcorded key).
	/// </summary>
	/// <returns>The <see cref="DarkLightMode"/> saved to local storage.</returns>
	public async Task<DarkLightMode> LoadUserPreferences()
	{
		return await _localStorageService.GetItemAsync<DarkLightMode>(Key);
	}

	/// <summary>
	/// Saves the user's dark or light mode preference to browser local storage.
	/// </summary>
	/// <param name="darkLightMode">The user's preferred mode.</param>
	public async Task SaveUserPreferences(DarkLightMode darkLightMode)
	{
		await _localStorageService.SetItemAsync(Key, darkLightMode);
	}
}
