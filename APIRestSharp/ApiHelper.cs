using RestSharp;

namespace APIRestSharp
{
    public static class ApiHelper
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
