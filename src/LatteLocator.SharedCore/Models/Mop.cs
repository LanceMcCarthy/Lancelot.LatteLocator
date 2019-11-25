using Newtonsoft.Json;

namespace LatteLocator.SharedCore.Models
{
    public class Mop
    {
        [JsonProperty("ready")]
        public bool Ready { get; set; }

        [JsonProperty("wait")]
        public object Wait { get; set; }
    }
}
