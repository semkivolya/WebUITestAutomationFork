using Newtonsoft.Json;

namespace TestAutomation.Business.Api
{
    public class Address
    {
        [JsonProperty("street")]
        public string? Street { get; set; }

        [JsonProperty("suite")]
        public string? Suite { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("zipcode")]
        public string? ZipCode { get; set; }

        [JsonProperty("geo")]

        public Geolocation? Location { get; set; }

    }
}
