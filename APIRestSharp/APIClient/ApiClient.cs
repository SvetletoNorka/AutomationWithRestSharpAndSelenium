using RestSharp;

namespace APIRestSharp.APIClient
{
    public class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public RestResponse ExecuteRequest(RestRequest request)
        {
            var response = _client.Execute(request);
            return response;
        }

        public async Task<RestResponse> ExecuteRequestAsync(RestRequest request)
        {
            var response = await _client.ExecuteAsync(request);
            return response;
        }
    }
}
