using System.Text.Json.Nodes;
using System.Text.Json;

namespace ASBNApp.Frontend.Helper
{
    public static class APIHelper
    {
        /// <summary>
        /// Helper method to extract the actual model from the API JSON response.
        /// </summary>
        /// <typeparam name="T">The type we want to return.</typeparam>
        /// <param name="content">JSON response content as string.</param>
        /// <returns></returns>
        public static T GetContent<T>(string content) where T : class
        {
            return JsonSerializer.Deserialize<JsonObject>(content)["value"].Deserialize<T>();
            //return JsonSerializer.Deserialize<T>((content)?."value");
        }
    }
}
