using ASBNApp.Frontend.Model;
using System.Text;
using System.Text.Json;

namespace ASBNApp.Frontend.Services
{
    public class ASBNDataService(HttpClient httpClient) : IASBNDataService
    {
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
                var json = await httpClient.GetStringAsync($"/odata/Entry?$filter=Date eq {date?.ToString("yyyy-MM-dd")}");
                var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModelWithID>>(json);
                
                return odata.value.FirstOrDefault() ?? new EntryRowModelWithID{ Date = (DateTime)date };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpsRequestException catched: {ex.Message}");
                return new EntryRowModelWithID { Date = (DateTime)date };
            }
        }

        public Settings? GetSettings()
        {
            throw new NotImplementedException();
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
                var json = await httpClient.GetStringAsync($"/odata/Entry?$filter=Date ge {startDate?.ToString("yyyy-MM-dd")} and Date le {endDate?.ToString("yyyy-MM-dd")}&$orderBy=Date");
                var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModelWithID>>(json);

                return odata.value;
            }
            catch (Exception ex)
            {
                return new List<EntryRowModelWithID>();
            }
        }

        /// <summary>
        /// Get all available <see cref="WorkLocationHours"/> objects.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkLocationHours>> GetWorkLocationHours()
        {
            try
            {
                var json = await httpClient.GetStringAsync($"/odata/WorkLocation");
                var odata = JsonSerializer.Deserialize<ODataBase<WorkLocationHours>>(json);

                return odata.value;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpsRequestException catched: {ex.Message}");
                return new List<WorkLocationHours>();
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
            var json = JsonSerializer.Serialize(entry as EntryRowModel);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            if (entry.Id == null)
            {
                HttpResponseMessage response = await httpClient.PostAsync($"/odata/Entry", content);
                return response.IsSuccessStatusCode;
            }
            else
            {
                HttpResponseMessage response = await httpClient.PatchAsync($"/odata/Entry({entry.Id})", content);
                return response.IsSuccessStatusCode;
            }
        }


        public async Task<bool> DeleteDay(int? id)
        {
            if(id == null) { return false; }

            HttpResponseMessage response = await httpClient.DeleteAsync($"/odata/Entry({id})");
            return response.IsSuccessStatusCode;
        }



        /// <summary>
        /// Handles saving data for an entire week.
        /// 
        /// Only saves entries where the <see cref="EntryRowModelWithID.Note"/> != null, 
        /// using POST / PATCH on the value for Id (if = null use POST, if != null use PATCH).
        /// </summary>
        /// <param name="entries">List of <see cref="EntryRowModelWithID"/>s to save.</param>
        /// <returns>True if success, false if error during saving.</returns>
        /// <exception cref="Exception">Throw exception if week wasn't saved successfully.</exception>
        public async Task<bool> SaveWeek(IEnumerable<EntryRowModelWithID> entries)
        {
            foreach(var entry in entries)
            {
                if(string.IsNullOrWhiteSpace(entry.Location))
                {
                    Console.WriteLine($"Nothing to save here, skipping entry for date {entry.Date}.");
                    continue;
                }

                // Serialize to EntryRowModel to get rid of the ID property 
                var json = JsonSerializer.Serialize(entry as EntryRowModel);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = (entry.Id == null) 
                    ? await httpClient.PostAsync($"/odata/Entry", content)
                    : await httpClient.PatchAsync($"/odata/Entry({entry.Id})", content);
                
                if(!response.IsSuccessStatusCode)
                {
                    return false;
                    throw new Exception($"Week only saved partially, couldn't save anything after entry with {entry.Date}.");
                }
            }

            return true;
        }

        public Task<bool> SaveSettings(Settings settings)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveWorkLocationHours(List<WorkLocationHours> workLocationHours)
        {
            throw new NotImplementedException();
        }
    }
}
