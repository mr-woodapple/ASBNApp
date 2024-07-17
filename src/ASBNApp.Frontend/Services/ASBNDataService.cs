using ASBNApp.Frontend.Model;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ASBNApp.Frontend.Services
{
    public class ASBNDataService(HttpClient httpClient) : IASBNDataService
    {
        /// <summary>
        /// Sends an OData request for the desired date.
        /// If the response object is null, return an empty EntryRowModel for the given day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns><see cref="EntryRowModel"/></returns>
        public async Task<EntryRowModel> GetDay(DateTime? date)
        {
            try
            {
                var json = await httpClient.GetStringAsync($"/odata/Entry?$filter=Date eq {date?.ToString("yyyy-MM-dd")}");
                var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModel>>(json);
                
                return odata.value.FirstOrDefault() ?? new EntryRowModel{ Date = (DateTime)date };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpsRequestException catched: {ex.Message}");
                return new EntryRowModel { Date = (DateTime)date };
            }
        }

        public Settings? GetSettings()
        {
            throw new NotImplementedException();
        }

        public async Task<List<EntryRowModel>> GetWeek(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var json = await httpClient.GetStringAsync($"/odata/Entry?$filter=Date ge {startDate?.ToString("yyyy-MM-dd")} and Date le {endDate?.ToString("yyyy-MM-dd")}");
                var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModel>>(json);

                return odata.value;
            }
            catch (Exception ex)
            {
                return new List<EntryRowModel>();
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

        public async Task<bool> SaveDay(EntryRowModel entry)
        {
            var json = JsonSerializer.Serialize(entry);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"/odata/Entry", content);

            return response.IsSuccessStatusCode ? true : false;
        }

        public async Task<bool> SaveWeek(IEnumerable<EntryRowModel> entries)
        {
            var json = JsonSerializer.Serialize(entries);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"/api/Entries/PostEntries", content);

            return response.IsSuccessStatusCode ? true : false;
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
