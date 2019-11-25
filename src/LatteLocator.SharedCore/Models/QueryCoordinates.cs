using Newtonsoft.Json;

namespace LatteLocator.SharedCore.Models
{
    public class QueryCoordinates
    {
        [JsonProperty("lat")]
        public float Lat { get; set; }

        [JsonProperty("lng")]
        public float Lng { get; set; }
    }
}
