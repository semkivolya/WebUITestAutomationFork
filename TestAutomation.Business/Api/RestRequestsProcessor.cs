using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RestSharp;
using TestAutomation.Core;

namespace TestAutomation.Business.Api
{
    public sealed class RestRequestsProcessor : IDisposable
    {
        private readonly ApiClient _apiClient;

        public RestRequestsProcessor(string baseUrl)
        {
            _apiClient = new ApiClient(baseUrl);
        }

        private static string UserJsonSchema => @"{
                'type': 'object',
                'required': ['id', 'name', 'username','email', 'address', 'phone', 'website', 'company']
            }";

        private static string UsersJsonSchema => @$"{{
                'type': 'array',
                'items':{UserJsonSchema}
            }}";

        public bool UsersDataIsValid(string? data)
        {
            if (string.IsNullOrEmpty(data)) return false;

            JSchema schema = JSchema.Parse(UsersJsonSchema);
            var users = JToken.Parse(data);
            return users.IsValid(schema);
        }

        public User[]? ConvertUsers(string? data)
        {
            return JsonConvert.DeserializeObject<User[]>(data);
        }

        public User? ConvertUser(string? data)
        {
            return JsonConvert.DeserializeObject<User>(data);
        }

        public Task<RestResponse> GetUsers()
        {
            var request = new RestRequestsBuilder("users")
                .SetMethod(Method.Get)
                .Build();
            return _apiClient.ExecuteAsync(request);
        }

        public Task<RestResponse> CreateUser(User user)
        {
            var request = new RestRequestsBuilder("users")
                .SetMethod(Method.Post)
                .AddJsonBody(user)
                .Build();
            return _apiClient.ExecuteAsync(request);
        }

        public Task<RestResponse> SendInvalidRequest()
        {
            var request = new RestRequestsBuilder("invalidendpoint")
                .SetMethod(Method.Get)
                .Build();
            return _apiClient.ExecuteAsync(request);
        }

        public void Dispose()
        {
            _apiClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
