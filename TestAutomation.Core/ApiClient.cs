using RestSharp;

namespace TestAutomation.Core
{
    public sealed class ApiClient : IDisposable
    {
        readonly RestClient _restClient;
        public ApiClient(string baseURL)
        {
            var options = new RestClientOptions() { BaseUrl = new Uri(baseURL) };
            _restClient = new RestClient(options, useClientFactory: true);
        }

        public Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            return _restClient.ExecuteAsync(request);
        }

        public void Dispose()
        {
            _restClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
