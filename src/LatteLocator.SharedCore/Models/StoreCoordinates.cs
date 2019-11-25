using Newtonsoft.Json;

namespace LatteLocator.SharedCore.Models
{
    public class StoreCoordinates
    {
        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }
    }
}
