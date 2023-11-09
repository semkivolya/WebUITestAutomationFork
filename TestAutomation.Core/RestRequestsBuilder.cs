using RestSharp;

namespace TestAutomation.Core
{
    public class RestRequestsBuilder
    {
        private readonly RestRequest _request;

        public RestRequestsBuilder(string resource)
        {
            _request = new RestRequest(resource);
        }

        public RestRequestsBuilder SetMethod(Method method)
        {
            _request.Method = method;
            return this;
        }

        public RestRequestsBuilder AddJsonBody(object payload)
        {
            _request.AddJsonBody(payload);
            return this;
        }

        public RestRequest Build()
        {
            return _request;
        }
    }
}
