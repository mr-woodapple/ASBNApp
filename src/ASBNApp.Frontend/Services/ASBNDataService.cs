using ASBNApp.Frontend.Helpers;
using ASBNApp.Frontend.Model;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASBNApp.Frontend.Services
{
    public class ASBNDataService : IASBNDataService
    {
        private readonly HttpClient _httpClient;

		public ASBNDataService(IHttpClientFactory httpClientFactory)
			=> _httpClient = httpClientFactory.CreateClient("BackendClient");


		/// <summary>
		/// Sends an OData request for the desired date.
		/// If the response object is null, return an empty EntryRowModelWithID for the given day.
		/// </summary>
		/// <param name="date">Date to request data for.</param>
		/// <returns>A single <see cref="EntryRowModelWithID"/> object.</returns>
		public async Task<EntryRowModelWithID> GetDay(DateTime? date)
        {
            try
            {
                var json = await _httpClient.GetStringAsync($"/api/odata/Entry?$filter=Date eq {date?.ToString("yyyy-MM-dd")}");
                var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModelWithID>>(json);
                
                return odata.value.FirstOrDefault() ?? new EntryRowModelWithID{ Date = (DateTime)date };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpsRequestException catched: {ex.Message}");
                return new EntryRowModelWithID { Date = (DateTime)date };
            }
        }

		/// <summary>
		/// Handles saving data for a day. Will use POST or PATCH depending
		/// on the value for Id, if = null: POST, if != null: PATCH.
		/// </summary>
		/// <param name="entry">The <see cref="EntryRowModelWithID"/> to save.</param>
		/// <returns>True for success, false for error during saving.</returns>
		public async Task<bool> SaveDay(EntryRowModelWithID entry)
		{
			var json = JsonSerializer.Serialize(DateTimeKindHelper.SetDateTimeKindToNone(entry) as EntryRowModel);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage response = (entry.Id == null)
				? await _httpClient.PostAsync("/api/odata/Entry", content)
				: await _httpClient.PatchAsync($"/api/odata/Entry({entry.Id})", content);

			return response.IsSuccessStatusCode;
		}

		/// <summary>
		/// Deletes the entry for a given ID.
		/// </summary>
		/// <param name="id">Id of the object to be deleted.</param>
		/// <returns>Bool, true representing success, false failure.</returns>
		public async Task<bool> DeleteDay(int? id)
		{
			if (id == null) { return false; }

			HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/odata/Entry({id})");
			return response.IsSuccessStatusCode;
		}

		/// <summary>
		/// Get all entries for a given timeframe.
		/// </summary>
		/// <param name="startDate">First day in the week.</param>
		/// <param name="endDate">Last day in the week.</param>
		/// <returns>List of <see cref="EntryRowModelWithID"/> objects.</returns>
		public async Task<List<EntryRowModelWithID>> GetWeek(DateTime? startDate, DateTime? endDate)
		{
			try
			{
				var json = await _httpClient.GetStringAsync($"/api/odata/Entry?$filter=Date ge {startDate?.ToString("yyyy-MM-dd")} and Date le {endDate?.ToString("yyyy-MM-dd")}&$orderBy=Date");
				var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModelWithID>>(json);

				return odata.value;
			}
			catch (Exception ex)
			{
				return new List<EntryRowModelWithID>();
			}
		}

		/// <summary>
		/// Handles saving data for an entire week (or any time span, really).
		/// 
		/// Only saves entries where the <see cref="EntryRowModelWithID.Note"/> != null, 
		/// using POST / PATCH on the value for Id (if = null use POST, if != null use PATCH).
		/// </summary>
		/// <param name="entries">List of <see cref="EntryRowModelWithID"/>s to save.</param>
		/// <returns>True if success, false if error during saving.</returns>
		/// <exception cref="Exception">Throw exception if week wasn't saved successfully.</exception>
		public async Task<bool> SaveEntries(IEnumerable<EntryRowModelWithID> entries)
		{
			foreach (var entry in entries)
			{
				if (string.IsNullOrWhiteSpace(entry.Note))
				{
					Console.WriteLine($"Nothing to save here, skipping entry for date {entry.Date}.");
					continue;
				}

				// Serialize to EntryRowModel to get rid of the ID property 
				var json = JsonSerializer.Serialize(DateTimeKindHelper.SetDateTimeKindToNone(entry) as EntryRowModel);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				HttpResponseMessage response = (entry.Id == null)
					? await _httpClient.PostAsync("/api/odata/Entry", content)
					: await _httpClient.PatchAsync($"/api/odata/Entry({entry.Id})", content);

				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Week only saved partially, couldn't save entry for {entry.Date} and following.");
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<List<EntryRowModelWithID>> GetAllEntries() {
            try
            {
                var json = await _httpClient.GetStringAsync($"/api/odata/Entry");
                var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModelWithID>>(json);

				return odata.value;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpsRequestException catched while trying to download all entries: {ex.Message}");
				return new List<EntryRowModelWithID>();
            }
        }

        /// <summary>
        /// Get all available <see cref="WorkLocation"/> objects.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkLocationWithID>> GetWorkLocationHours()
		{
			try
			{
				var json = await _httpClient.GetStringAsync($"/api/odata/WorkLocation");
				var odata = JsonSerializer.Deserialize<ODataBase<WorkLocationWithID>>(json);

				return odata.value;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"HttpsRequestException catched: {ex.Message}");
				return new List<WorkLocationWithID>();
			}
		}

		/// <summary>
		/// Handles saving a <see cref="WorkLocation"/> object with the API.
		/// </summary>
		/// <param name="workLocationHours">The <see cref="WorkLocation"/> to save.</param>
		/// <returns>Bool, true representing success, false failure.</returns>
		public async Task<bool> SaveWorkLocationHours(List<WorkLocationWithID> workLocationHours)
        {
            foreach (var entry in workLocationHours)
            {
                var json = JsonSerializer.Serialize(entry as WorkLocation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = (entry.Id == null)
                    ? await _httpClient.PostAsync("/api/odata/WorkLocation", content)
                    : await _httpClient.PatchAsync($"/api/odata/WorkLocation({entry.Id})", content);

                if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"""Work locations only saved partially, couldn't save anything after entry for location "{entry.Location}".""");
					return false;
				}
			}

            return true;
        }

		/// <summary>
		/// Delete a <see cref="WorkLocation"/> entry for the given id.
		/// </summary>
		/// <param name="id">The id of the object to be deleted.</param>
		/// <returns>Bool, true representing success, false failure.</returns>
		public async Task<bool> DeleteWorkLocationHours(int? id)
        {
			if (id == null) { return false; }

			HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/odata/WorkLocation({id})");
			return response.IsSuccessStatusCode;
        }

		/// <summary>
		/// Returns a <see cref="Settings"/> object to be displayed on the settings page (for example).
		/// </summary>
		/// <returns><see cref="Settings"/> object.</returns>
		public async Task<Settings> GetSettings()
		{
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var json = await _httpClient.GetStringAsync("/api/Settings");
			var formattedData = JsonSerializer.Deserialize<Settings>(json, options);

			return formattedData;
		}

		/// <summary>
		/// Saves a <see cref="Settings"/> object by sending it off to the API.
		/// </summary>
		/// <param name="settings">The settings model to save.</param>
		/// <returns>Bool, true representing success, false failure.</returns>
		public async Task<bool> SaveSettings(Settings settings)
		{
			var json = JsonSerializer.Serialize(settings);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PatchAsync("/api/Settings", content);
			return response.IsSuccessStatusCode;
		}

		/// <summary>
		/// Send the data to import to the /odata/Import API endpoint.
		/// </summary>
		/// <param name="data"><see cref="JSONDataWrapper"/> model to import.</param>
		/// <returns>An <see cref="HttpResponseMessage"/>.</returns>
		public async Task<HttpResponseMessage> ImportData(JSONDataWrapper data)
		{
			var json = JsonSerializer.Serialize(data);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("/api/odata/Import", content);
			return response;
		}
	}
}
