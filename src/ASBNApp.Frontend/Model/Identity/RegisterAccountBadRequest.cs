using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASBNApp.Frontend.Model.Identity
{
	public class RegisterAccountBadRequest
	{
		public string type { get; set; }
		public string title { get; set; }
		public int status { get; set; }
		public string detail { get; set; }
		public string instance { get; set; }

		[JsonExtensionData]
		public Dictionary<string, JsonElement> Errors { get; set; }
	}
}
