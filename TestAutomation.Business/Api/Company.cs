using Newtonsoft.Json;

namespace TestAutomation.Business.Api
{
    public class Company
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("catchPhrase")]
        public string? CatchPhrase { get; set; }


        [JsonProperty("bs")]
        public string? BusinessStrategy { get; set; }
    }
}
