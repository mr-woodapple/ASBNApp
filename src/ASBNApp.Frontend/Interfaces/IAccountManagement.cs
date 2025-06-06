﻿using ASBNApp.Frontend.Models;

namespace ASBNApp.Frontend.Interfaces;

public interface IAccountManagement
{
	/// <summary>
	/// Login service.
	/// </summary>
	/// <param name="email">User's email.</param>
	/// <param name="password">User's password.</param>
	/// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
	Task<HttpResponseMessage> LoginAsync(UserAccount userAccount);

	/// <summary>
	/// Log out the logged in user.
	/// </summary>
	/// <returns>The asynchronous task.</returns>
	Task LogoutAsync();

	/// <summary>
	/// Registration service.
	/// </summary>
	/// <param name="email">User's email.</param>
	/// <param name="password">User's password.</param>
	/// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
	Task<HttpResponseMessage> RegisterAsync(UserAccount userAccount);

	Task<bool> CheckAuthenticatedAsync();
}

/// <summary>
/// Response for login and registration.
/// </summary>
public class FormResult
{
	/// <summary>
	/// Gets or sets a value indicating whether the action was successful.
	/// </summary>
	public bool Succeeded { get; set; }

	/// <summary>
	/// On failure, the problem details are parsed and returned in this array.
	/// </summary>
	public string[] ErrorList { get; set; } = [];
}