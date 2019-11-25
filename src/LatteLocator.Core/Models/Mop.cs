using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class Mop
    {
        [JsonProperty("ready")]
        public bool Ready { get; set; }

        [JsonProperty("wait")]
        public object Wait { get; set; }
    }
}
