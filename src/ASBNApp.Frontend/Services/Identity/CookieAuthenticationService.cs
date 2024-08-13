﻿using System.Text;
using System.Text.Json;
using ASBNApp.Frontend.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;


namespace ASBNApp.Frontend.Services
{
	/// <summary>
	/// Handles cookie-based authentication and provides information about the auth state.
	/// </summary>
	public class CookieAuthenticationService : AuthenticationStateProvider, IAccountManagement
	{
		/// <summary>
		/// Map the JavaScript-formatted properties to C#-formatted classes.
		/// </summary>
		private readonly JsonSerializerOptions jsonSerializerOptions =
			new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

		/// <summary>
		/// Authentication state.
		/// </summary>
		private bool _authenticated = false;

		/// <summary>
		/// Default principal for anonymous (not authenticated) users.
		/// </summary>
		private readonly ClaimsPrincipal Unauthenticated =
			new(new ClaimsIdentity());

		/// <summary>
		/// Special auth client.
		/// </summary>
		private readonly HttpClient _httpClient;

		/// <summary>
		/// Create a new instance of the auth provider.
		/// </summary>
		/// <param name="httpClientFactory">Factory to retrieve auth client.</param>
		public CookieAuthenticationService(IHttpClientFactory httpClientFactory)
			=> _httpClient = httpClientFactory.CreateClient("Auth");


		public async Task LoginAsync(UserAccount userAccount)
		{
			var json = JsonSerializer.Serialize(userAccount);
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync("/login?useCookies=true", content);

			if (response.IsSuccessStatusCode)
			{
				// need to refresh auth state
				NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
			}

			return;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			_authenticated = false;
			var user = Unauthenticated;

			try
			{
				var userResponse = await _httpClient.GetAsync("https://localhost:7148/manage/info");
				userResponse.EnsureSuccessStatusCode();

				// user is authenticated,so let's build their authenticated identity
				var userJson = await userResponse.Content.ReadAsStringAsync();
				var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);

				if (userInfo != null)
				{
					// create the user object:
					var claims = new List<Claim>
					{
						new(ClaimTypes.Name, userInfo.Email),
						new(ClaimTypes.Email, userInfo.Email)
					};

					// add any additional claims
					claims.AddRange(
						userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
							.Select(c => new Claim(c.Key, c.Value)));

					// If we need roles, these could be added to the claim here.

					var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationService));
					user = new ClaimsPrincipal(id);
					_authenticated = true;
				}
			}
			catch { }
			
			return new AuthenticationState(user);
		}

		// Only used when logging out
		public async Task<bool> CheckAuthenticatedAsync()
		{
			await GetAuthenticationStateAsync();
			return _authenticated;
		}

		

		public Task LogoutAsync()
		{
			throw new NotImplementedException();
		}

		public Task<FormResult> RegisterAsync(string email, string password)
		{
			throw new NotImplementedException();
		}
	}
}
