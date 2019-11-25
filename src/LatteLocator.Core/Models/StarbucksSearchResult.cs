using System.Collections.Generic;
using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class StarbucksSearchResult
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("stores")]
        public List<Store> Stores { get; set; }

        [JsonProperty("coordinates")]
        public QueryCoordinates Coordinates { get; set; }

        [JsonProperty("includesRecommendedLocations")]
        public bool IncludesRecommendedLocations { get; set; }
    }
}
