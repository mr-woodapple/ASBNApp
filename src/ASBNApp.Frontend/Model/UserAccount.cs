using System.Text.Json.Serialization;

namespace ASBNApp.Frontend.Model
{
	/// <summary>
	/// Simple model used to serialize the login data correctly.
	/// </summary>
	public class UserAccount
	{
		[JsonPropertyName("email")]
		public string EMail { get; set; }

		[JsonPropertyName("password")]
		public string Password { get; set; }
	}
}
