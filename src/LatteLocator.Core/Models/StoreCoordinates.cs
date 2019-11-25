using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class StoreCoordinates
    {
        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }
    }
}
