using Newtonsoft.Json;

namespace LatteLocator.Core.Models
{
    public class Schedule
    {
        [JsonProperty("dayName")]
        public string DayName { get; set; }

        [JsonProperty("hours")]
        public string Hours { get; set; }

        [JsonProperty("open")]
        public bool? Open { get; set; }

        [JsonProperty("holiday")]
        public string Holiday { get; set; }
    }
}
