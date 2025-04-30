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

	public async Task<DarkLightMode> LoadUserPreferences()
	{
		return await _localStorageService.GetItemAsync<DarkLightMode>(Key);
	}

	public async Task SaveUserPreferences(DarkLightMode darkLightMode)
	{
		await _localStorageService.SetItemAsync(Key, darkLightMode);
	}
}
