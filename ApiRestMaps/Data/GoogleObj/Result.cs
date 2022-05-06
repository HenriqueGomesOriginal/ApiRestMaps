using System.Text.Json.Serialization;

namespace ApiRestMaps.Data.GoogleObj
{
    public class Result
    {
        [JsonPropertyName("address_components")]
        public List<AddressComponents> AddressComponents { get; set; }
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }
        public List<String> Types { get; set; }
    }
}
