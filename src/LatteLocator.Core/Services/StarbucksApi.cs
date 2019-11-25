using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LatteLocator.Core.Common;
using LatteLocator.Core.Models;
using Newtonsoft.Json;

namespace LatteLocator.Core.Services
{
    public class StarbucksApi : IDisposable
    {
        private readonly HttpClient client;

        public StarbucksApi(bool useDataCompression = true)
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression && useDataCompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public async Task<StarbucksSearchResult> SearchNearbyStores(double longitude, double latitude)
        {
            // example request: https://www.starbucks.com/bff/locations?lat=42.369013&lng=-71.02005
            // feature filter https://www.starbucks.com/bff/locations?features=NB&lat=42.369013&lng=-71.02005
            // multifeature filter https://www.starbucks.com/bff/locations?features=NB,DT&lat=42.369013&lng=-71.02005

            var uriString = $"{Constants.UpdatedStarbucksEndpoint}?" +
                            $"lat={latitude.ToString(CultureInfo.InvariantCulture)}&" +
                            $"lng={longitude.ToString(CultureInfo.InvariantCulture)}";
            
            var result = await client.GetStringAsync(new Uri(uriString, UriKind.RelativeOrAbsolute));

            return JsonConvert.DeserializeObject<StarbucksSearchResult>(result);
        }

        public async Task<StarbucksSearchResult> FilterNearbyStores(double longitude, double latitude, string features)
        {
            // example request: https://www.starbucks.com/bff/locations?lat=42.369013&lng=-71.02005
            // feature filter https://www.starbucks.com/bff/locations?features=NB&lat=42.369013&lng=-71.02005
            // multifeature filter https://www.starbucks.com/bff/locations?features=NB,DT&lat=42.369013&lng=-71.02005

            var uriString = $"{Constants.UpdatedStarbucksEndpoint}?" +
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
