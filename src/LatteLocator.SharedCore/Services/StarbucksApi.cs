using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LatteLocator.SharedCore.Models;
using Newtonsoft.Json;

namespace LatteLocator.SharedCore.Services
{
    public class StarbucksApi : IDisposable
    {
        private readonly string apiEndpoint = "https://www.starbucks.com/bff/locations";
        private readonly HttpClient client;

        /// <summary>
        /// Starbucks API Service.
        /// </summary>
        /// <param name="useDataCompression">Default is true. Flag to use GZIP or Deflate http response compression</param>
        public StarbucksApi(bool useDataCompression = true)
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression && useDataCompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        /// <summary>
        /// Starbucks API Service. This overload is only used when you need to override the default root URL for the API endpoint
        /// </summary>
        /// <param name="apiRootOverride">The string URL for the API root.</param>
        /// <param name="useDataCompression">Default is true. Flag to use GZIP or Deflate http response compression</param>
        public StarbucksApi(string apiRootOverride, bool useDataCompression = true)
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression && useDataCompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

            this.apiEndpoint = apiRootOverride;
        }

        public virtual async Task<StarbucksSearchResult> SearchNearbyStores(double longitude, double latitude)
        {
            // example request: https://www.starbucks.com/bff/locations?lat=42.369013&lng=-71.02005
            // feature filter https://www.starbucks.com/bff/locations?features=NB&lat=42.369013&lng=-71.02005
            // multifeature filter https://www.starbucks.com/bff/locations?features=NB,DT&lat=42.369013&lng=-71.02005

            var uriString = $"{apiEndpoint}?" +
                            $"lat={latitude.ToString(CultureInfo.InvariantCulture)}&" +
                            $"lng={longitude.ToString(CultureInfo.InvariantCulture)}";
            
            var result = await client.GetStringAsync(new Uri(uriString, UriKind.RelativeOrAbsolute));

            return JsonConvert.DeserializeObject<StarbucksSearchResult>(result);
        }

        public virtual async Task<StarbucksSearchResult> FilterNearbyStores(double longitude, double latitude, string features)
        {
            // example request: https://www.starbucks.com/bff/locations?lat=42.369013&lng=-71.02005
            // feature filter https://www.starbucks.com/bff/locations?features=NB&lat=42.369013&lng=-71.02005
            // multifeature filter https://www.starbucks.com/bff/locations?features=NB,DT&lat=42.369013&lng=-71.02005

            var uriString = $"{apiEndpoint}?" +
                $"features={features}&" +
                $"lat={latitude.ToString(CultureInfo.InvariantCulture)}&" +
                $"lng={longitude.ToString(CultureInfo.InvariantCulture)}";

            var result = await client.GetStringAsync(new Uri(uriString, UriKind.RelativeOrAbsolute));

            return JsonConvert.DeserializeObject<StarbucksSearchResult>(result);
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
