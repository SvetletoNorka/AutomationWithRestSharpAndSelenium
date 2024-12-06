using RestSharp;

namespace APIRestSharp.APIClient
{
    public class RestClientHelper
    {
        public static RestRequest CreateRequest(string endpoint, Method method, object body = null)
        {
            var request = new RestRequest(endpoint, method);

            if (body != null)
            {
                request.AddJsonBody(body);
            }

            return request;
        }
    }
}
