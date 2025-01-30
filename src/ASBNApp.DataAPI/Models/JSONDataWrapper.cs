namespace ASBNApp.DataAPI.Models
{
    public class JSONDataWrapper
    {
        public int? ID { get; set; }
        public Settings? Settings { get; set; }
        public List<WorkLocation>? WorkLocationHours { get; set; }
        public List<Entry>? Data { get; set; }
    }
}