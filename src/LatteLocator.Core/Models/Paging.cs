using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class Paging
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("returned")]
        public int Returned { get; set; }
    }
}
