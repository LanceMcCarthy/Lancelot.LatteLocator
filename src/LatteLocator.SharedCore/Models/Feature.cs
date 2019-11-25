using Newtonsoft.Json;

namespace LatteLocator.SharedCore.Models
{
    public class Feature
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
