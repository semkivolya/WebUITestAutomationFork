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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            Logger.Info("Sent GET request to 'users' endpoint");
            return _apiClient.ExecuteAsync(request);
        }

        public Task<RestResponse> CreateUser(User user)
        {
            var request = new RestRequestsBuilder("users")
                .SetMethod(Method.Post)
                .AddJsonBody(user)
                .Build();
            Logger.Info($"Sent POST request to 'users' endpoint with data: {JsonConvert.SerializeObject(user)}");
            return _apiClient.ExecuteAsync(request);
        }

        public Task<RestResponse> SendInvalidRequest()
        {
            var request = new RestRequestsBuilder("invalidendpoint")
                .SetMethod(Method.Get)
                .Build();
            Logger.Info("Sent GET request to 'invalidendpoint' endpoint");
            return _apiClient.ExecuteAsync(request);
        }

        public void Dispose()
        {
            _apiClient?.Dispose();
            Logger.Info("Dispose ApiClient");
            GC.SuppressFinalize(this);
        }
    }
}
