using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KayeeDictionaryServiceDemo.Services.Dictionary.Models
{
    public class DictionaryEndpointResponse
    {
        [JsonPropertyName("lang")] public string Lang { get; set; }

        [JsonPropertyName("app")] public string App { get; set; }

        [JsonPropertyName("phrases")] public Dictionary<string, string> Phrases { get; set; }
    }
}