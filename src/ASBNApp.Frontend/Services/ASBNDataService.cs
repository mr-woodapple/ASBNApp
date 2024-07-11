using ASBNApp.Frontend.Helper;
using ASBNApp.Frontend.Model;
using ASBNApp.Model;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ASBNApp.Frontend.Services
{
    public class ASBNDataService(HttpClient httpClient) : IASBNDataService
    {
        public async Task<EntryRowModel> GetDay(DateTime? date)
        {
            try
            {
                var json = await httpClient.GetStringAsync($"/odata/Entries?$filter=Date eq {date?.ToString("yyyy-MM-dd")}");
                var odata = JsonSerializer.Deserialize<ODataBase<EntryRowModel>>(json);
                
                return odata.value.FirstOrDefault();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpsRequestException catched: {ex.Message}");
                return new EntryRowModel();
            }
        }

        public Settings? GetSettings()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntryRowModel> GetWeek(int? year, int? week)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkLocationHours>> GetWorkLocationHours()
        {
            try
            {
                var json = await httpClient.GetStringAsync($"/odata/WorkLocations");
                var odata = JsonSerializer.Deserialize<ODataBase<WorkLocationHours>>(json);

                return odata.value;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpsRequestException catched: {ex.Message}");
                return new List<WorkLocationHours>();
            }
        }

        public Task ReadData()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveDay(string Note, DateTime Date, string Location, float Hours)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveSettings(Settings settings)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveWeek(IEnumerable<EntryRowModel> entries)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveWorkLocationHours(List<WorkLocationHours> workLocationHours)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteData()
        {
            throw new NotImplementedException();
        }
    }
}
