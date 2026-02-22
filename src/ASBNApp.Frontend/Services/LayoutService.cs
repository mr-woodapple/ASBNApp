using MudBlazor;
using ASBNApp.Frontend.Enums;
using ASBNApp.Frontend.Interfaces;

namespace ASBNApp.Frontend.Services;

public class LayoutService
{
	private bool _systemDarkMode;
	public MudTheme mudTheme = DefaultTheme.GetTheme();
	private DarkLightMode _userPreferredDarkLightMode;
	private readonly IUserPreferenceService _userPreferencesService;

	public bool ObserveSystemThemeChange { get; private set; } = true;

	/// <summary>
	/// The user's preferred dark/light mode setting.
	/// This preference is used to determine the actual <see cref="IsDarkMode"/> state.
	/// </summary>
	public DarkLightMode CurrentDarkLightMode { get; private set; } = DarkLightMode.System;

	/// <summary>
	/// Dark mode is currently active.
	/// This is determined by <see cref="UpdateDarkModeAsync"/> based on user and system preferences and should not be modified directly.
	/// </summary>
	public bool IsDarkMode { get; private set; }

	/// <summary>
	/// Observes system theme changes to update dark/light mode.
	/// </summary>
	public bool ObserveSystemDarkModeChange { get; private set; }

	/// <summary>
	/// The currently active MudBlazor theme.
	/// </summary>
	public MudTheme CurrentTheme { get; private set; }

	public LayoutService(IUserPreferenceService userPreferenceService)
	{
		_userPreferencesService = userPreferenceService;
	}

	public event EventHandler MajorUpdateOccurred;
	private void OnMajorUpdateOccurred() => MajorUpdateOccurred?.Invoke(this, EventArgs.Empty);

	/// <summary>
	/// Updates the dark mode state based on user preference and, optionally, the system's dark mode setting.
	/// </summary>
	/// <param name="systemMode">The current system dark mode setting. If <c>null</c>, the existing known system mode is used.</param>
	public void UpdateDarkModeState(bool? systemMode = null)
	{
		if (systemMode.HasValue)
		{
			_systemDarkMode = systemMode.Value;
		}

		IsDarkMode = CurrentDarkLightMode switch
		{
			DarkLightMode.Dark => true,
			DarkLightMode.Light => false,
			_ => _systemDarkMode,
		};
	}


	/// <summary>
	/// Handles changes in the system's dark mode setting.
	/// </summary>
	/// <param name="isSystemDarkMode"><c>true</c> if the system is in dark mode, otherwise <c>false</c>.</param>
	public Task OnSystemModeChangedAsync(bool isSystemDarkMode)
	{
		_systemDarkMode = isSystemDarkMode;
		UpdateDarkModeState();
		OnMajorUpdateOccurred();
		return Task.CompletedTask;
	}


	/// <summary>
	/// Applies user preferences for dark/light mode.
	/// Loads preferences from the user preference service and updates the theme accordingly.
	/// </summary>
	/// <param name="isDarkModeDefaultTheme">Indicates whether the default theme is dark mode.</param>
	public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
	{
		_systemDarkMode = isDarkModeDefaultTheme;
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

	/// <summary>
	/// Cycles through the dark/light mode options (System -> Light -> Dark -> System).
	/// Updates the current mode and saves the preference using the user preference service.
	/// </summary>
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
				IsDarkMode = _systemDarkMode;
				break;
		}

		await _userPreferencesService.SaveUserPreferences(CurrentDarkLightMode);
		OnMajorUpdateOccurred();
	}
}
