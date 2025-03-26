using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASBNApp.Frontend.Models.Identity
{
	public class RegisterAccountBadRequest
	{
		public string? type { get; set; }
		public string? title { get; set; }
		public int? status { get; set; }
		public string? detail { get; set; }
		public string? instance { get; set; }

		public Dictionary<string, List<string>> errors { get; set; }
	}
}
