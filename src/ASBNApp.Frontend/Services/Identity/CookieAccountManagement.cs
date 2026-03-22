using System.Text;
using System.Text.Json;
using ASBNApp.Frontend.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ASBNApp.Frontend.Interfaces;


namespace ASBNApp.Frontend.Services
{
	/// <summary>
	/// Handles cookie-based authentication and provides information about the auth state.
	/// </summary>
	/// <remarks>
	/// Create a new instance of the auth provider.
	/// </remarks>
	/// <param name="httpClientFactory">Factory to retrieve auth client.</param>
	public class CookieAccountManagement(
		IHttpClientFactory httpClientFactory, 
		ILogger<CookieAccountManagement> logger) : AuthenticationStateProvider, IAccountManagement
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
		private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

		/// <summary>
		/// Special auth client.
		/// </summary>
		private readonly HttpClient _httpClient = httpClientFactory.CreateClient("BackendClient");

		/// <summary>
		/// Handle registering the user with the api.
		/// </summary>
		/// <param name="email">The users e-mail address.</param>
		/// <param name="password">The users password.</param>
		public async Task<HttpResponseMessage> RegisterAsync(UserAccount userAccount)
		{
			var json = JsonSerializer.Serialize(userAccount);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync("/api/register", content);

			return response;
		}

		/// <summary>
		/// Handle login with the api and update the authentication state.
		/// </summary>
		/// <param name="userAccount">The user account to login.</param>
		/// <returns>The requests response message.</returns>
		public async Task<HttpResponseMessage> LoginAsync(UserAccount userAccount)
		{
			var json = JsonSerializer.Serialize(userAccount);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync("/api/login?useCookies=true", content);

			if (response.IsSuccessStatusCode)
			{
				// Need to refresh auth state
				NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
			}

			return response;
		}

		/// <summary>
		/// Handle logout with the api and update the authentication state.
		/// </summary>
		public async Task LogoutAsync()
		{
			const string empty = "{}";
			var emptyContent = new StringContent(empty, Encoding.UTF8, "application/json");
			await _httpClient.PostAsync("/api/logout", emptyContent);
			
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		/// <summary>
		/// Update the authentication state used across the app.
		/// </summary>
		/// <returns>The updated <see cref="AuthenticationState"/>, if any.</returns>
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			_authenticated = false;
			var user = Unauthenticated;

			try
			{
				var userResponse = await _httpClient.GetAsync("/api/manage/info");
				userResponse.EnsureSuccessStatusCode();

				// User is authenticated,so let's build their authenticated identity
				var userJson = await userResponse.Content.ReadAsStringAsync();
				var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);

				if (userInfo != null)
				{
					// Create the user object:
					var claims = new List<Claim>
					{
						new(ClaimTypes.Name, userInfo.Email),
						new(ClaimTypes.Email, userInfo.Email)
					};

					// Add any additional claims
					claims.AddRange(
						userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
							.Select(c => new Claim(c.Key, c.Value)));

					var id = new ClaimsIdentity(claims, nameof(CookieAccountManagement));
					user = new ClaimsPrincipal(id);
					_authenticated = true;
				}
			}
			catch 
			{
				logger.LogError("Failed retrieving user authentication state. User is not logged in.");
			}
			
			return new AuthenticationState(user);
		}

		/// <summary>
		/// Only used when logging out.
		/// </summary>
		/// <returns>True if authenticated, false if not.</returns>
		public async Task<bool> CheckAuthenticatedAsync()
		{
			await GetAuthenticationStateAsync();
			return _authenticated;
		}
	}
}
