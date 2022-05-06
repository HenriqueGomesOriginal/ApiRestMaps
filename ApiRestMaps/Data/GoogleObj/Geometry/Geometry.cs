using System.Text.Json.Serialization;

namespace ApiRestMaps.Data.GoogleObj
{
    public class Geometry
    {
        public Bounds Bounds { get; set; }
        public Coordinates Location { get; set; }
        [JsonPropertyName("location_type")]
        public string LocationType { get; set; }
        public Bounds ViewPort { get; set; }
    }
}
