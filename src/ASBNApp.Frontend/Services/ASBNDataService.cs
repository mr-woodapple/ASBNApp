using ASBNApp.Frontend.Helpers;
using ASBNApp.Frontend.Model;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
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
        /// <param name="date"></param>
        /// <returns><see cref="EntryRowModelWithID"/></returns>
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

        public async Task SaveWeek(IEnumerable<EntryRowModelWithID> entries)
        {
            // - Build logic here that only saves the entries that aren't "null"
            // - POST entries without an ID property
            // - PUT / PATCH entries with an ID property

            foreach(var entry in entries)
            {
                if(string.IsNullOrWhiteSpace(entry.Location))
                {
                    break;
                }

                // Serialize to EntryRowModelWithID to get rid of the ID property 
                var json = JsonSerializer.Serialize(entry as EntryRowModel);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                if (entry.Id == null)
                {
                    // POST
                    HttpResponseMessage response = await httpClient.PostAsync($"/odata/Entry", content);
                }
                else
                {
                    // PATCH
                    HttpResponseMessage response = await httpClient.PatchAsync($"/odata/Entry({entry.Id})", content);
                    //return response.IsSuccessStatusCode ? true : false;
                }
            }

            // return message hinzufügen
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
