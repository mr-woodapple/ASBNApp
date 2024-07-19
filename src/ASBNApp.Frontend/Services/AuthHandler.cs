using ASBNApp.Frontend.Model;
using System.Text.Json;
using System.Text;

namespace ASBNApp.Frontend.Services
{
	public class AuthHandler(HttpClient httpClient)
	{
		public async Task Login(LoginWrapper loginData)
		{
			var json = JsonSerializer.Serialize(loginData);
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			
			HttpResponseMessage response = await httpClient.PostAsync($"/login?useCookies=true&useSessionCookies=true", content);
			//return response.IsSuccessStatusCode;
		}
	}
}
