using System.Text.Json.Serialization;

namespace ASBNApp.Frontend.Model
{
    /// <summary>
    /// Base model for working with the OData responses (easier to deserialize).
    /// </summary>
    /// <typeparam name="T">The type that <see cref="value"/> should be from.</typeparam>
    public class ODataBase<T>
    {
        [JsonPropertyName("@odata.context")]
        public string Metadata { get; set; }
        public List<T> value { get; set; }
    }
}
