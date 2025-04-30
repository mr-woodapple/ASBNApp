using MudBlazor;
using ASBNApp.Frontend.Enums;
using ASBNApp.Frontend.Interfaces;

namespace ASBNApp.Frontend.Services;

public class LayoutService
{
	private bool _systemPreferences;
	public MudTheme mudTheme = DefaultTheme.GetTheme();
	private DarkLightMode _userPreferredDarkLightMode;
	private readonly IUserPreferenceService _userPreferencesService;

	public bool IsDarkMode { get; private set; }
	public bool ObserveSystemThemeChange { get; private set; } = true;
	public DarkLightMode CurrentDarkLightMode { get; private set; } = DarkLightMode.System;

	public LayoutService(IUserPreferenceService userPreferenceService)
	{
		_userPreferencesService = userPreferenceService;
	}

	public event EventHandler MajorUpdateOccurred;
	private void OnMajorUpdateOccurred() => MajorUpdateOccurred?.Invoke(this, EventArgs.Empty);

	public Task OnSystemPreferenceChanged(bool newValue)

	{
		_systemPreferences = newValue;

		if (CurrentDarkLightMode == DarkLightMode.System)
		{
			IsDarkMode = newValue;
			OnMajorUpdateOccurred();
		}

		return Task.CompletedTask;
	}

	/// TODO: check if this is really required
	public void SetDarkMode(bool value)
	{
		IsDarkMode = value;
	}

	public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
	{
		_systemPreferences = isDarkModeDefaultTheme;
		_userPreferredDarkLightMode = await _userPreferencesService.LoadUserPreferences();

		if (_userPreferredDarkLightMode != null)
		{
			CurrentDarkLightMode = _userPreferredDarkLightMode;
			IsDarkMode = CurrentDarkLightMode switch
			{
				DarkLightMode.Dark => true,
				DarkLightMode.Light => false,
				DarkLightMode.System => isDarkModeDefaultTheme,
				_ => IsDarkMode
			};
		}
		else
		{
			IsDarkMode = isDarkModeDefaultTheme;
			_userPreferredDarkLightMode = DarkLightMode.System;
			await _userPreferencesService.SaveUserPreferences(_userPreferredDarkLightMode);
		}
	}

	public async Task CycleDarkLightModeAsync()
	{
		switch (CurrentDarkLightMode)
		{
			// Change to Light
			case DarkLightMode.System:
				CurrentDarkLightMode = DarkLightMode.Light;
				ObserveSystemThemeChange = false;
				IsDarkMode = false;
				break;
			// Change to Dark
			case DarkLightMode.Light:
				CurrentDarkLightMode = DarkLightMode.Dark;
				ObserveSystemThemeChange = false;
				IsDarkMode = true;
				break;
			// Change to System
			case DarkLightMode.Dark:
				CurrentDarkLightMode = DarkLightMode.System;
				ObserveSystemThemeChange = true;
				IsDarkMode = _systemPreferences;
				break;
		}

		await _userPreferencesService.SaveUserPreferences(CurrentDarkLightMode);
		OnMajorUpdateOccurred();
	}
}
