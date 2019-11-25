using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class QueryCoordinates
    {
        [JsonProperty("lat")]
        public float Lat { get; set; }

        [JsonProperty("lng")]
        public float Lng { get; set; }
    }
}
