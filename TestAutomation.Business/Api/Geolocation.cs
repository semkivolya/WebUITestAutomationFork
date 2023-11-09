using Newtonsoft.Json;

namespace TestAutomation.Business.Api
{
    public class Geolocation
    {
        [JsonProperty("lat")]
        public string? Longitude { get; set; }

        [JsonProperty("lng")]
        public string? Latitude { get; set; }
    }
}
