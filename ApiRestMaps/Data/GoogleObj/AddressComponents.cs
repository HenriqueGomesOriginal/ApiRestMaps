using System.Text.Json.Serialization;

namespace ApiRestMaps.Data.GoogleObj
{
    public class AddressComponents
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; }
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }
        public List<String> Types { get; set; }
    }
}
