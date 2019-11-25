using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class Address
    {
        [JsonProperty("streetAddressLine1")]
        public string StreetAddressLine1 { get; set; }

        [JsonProperty("streetAddressLine2")]
        public string StreetAddressLine2 { get; set; }

        [JsonProperty("streetAddressLine3")]
        public string StreetAddressLine3 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("countrySubdivisionCode")]
        public string CountrySubdivisionCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }
}
