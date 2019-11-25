﻿using Newtonsoft.Json;

namespace LatteLocator.SharedCore.Models
{
    public class Timezoneinfo
    {
        [JsonProperty("currentTimeOffset")]
        public int CurrentTimeOffset { get; set; }

        [JsonProperty("windowsTimeZoneId")]
        public string WindowsTimeZoneId { get; set; }

        [JsonProperty("olsonTimeZoneId")]
        public string OlsonTimeZoneId { get; set; }
    }
}
