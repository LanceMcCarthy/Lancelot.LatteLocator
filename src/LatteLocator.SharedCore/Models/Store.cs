using System.Collections.Generic;
using Newtonsoft.Json;

namespace LatteLocator.SharedCore.Models
{
    public class Store
    {
        [JsonProperty("recommendation")]
        public Recommendation Recommendation { get; set; }

        [JsonProperty("storeNumber")]
        public string StoreNumber { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("coordinates")]
        public StoreCoordinates Coordinates { get; set; }

        [JsonProperty("regulations")]
        public List<object> Regulations { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("timeZoneInfo")]
        public Timezoneinfo TimeZoneInfo { get; set; }

        [JsonProperty("brandName")]
        public string BrandName { get; set; }

        [JsonProperty("ownershipTypeCode")]
        public string OwnershipTypeCode { get; set; }

        [JsonProperty("open")]
        public bool Open { get; set; }

        [JsonProperty("openStatusText")]
        public string OpenStatusText { get; set; }

        [JsonProperty("addressLines")]
        public List<string> AddressLines { get; set; }

        [JsonProperty("mop")]
        public Mop Mop { get; set; }

        [JsonProperty("schedule")]
        public List<Schedule> Schedule { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }
}
