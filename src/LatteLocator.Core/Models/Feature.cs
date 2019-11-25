using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class Feature
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
