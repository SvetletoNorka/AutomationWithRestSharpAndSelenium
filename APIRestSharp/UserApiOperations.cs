using Newtonsoft.Json.Linq;
using RestSharp;

namespace APIRestSharp.APIClient
{
    public class ApiUsersOperations
    {
        private readonly ApiClient _apiClient;

        public ApiUsersOperations(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<JObject> GetAllUsers()
        {
            var allUsers = new List<JObject>();
            int currentPage = 1;
            int totalPages = 0;

            do
            {
                var users = GetUsersFromPage(currentPage);
                allUsers.AddRange(users);

                if (currentPage == 1)
                {
                    var request = new RestRequest($"api/users?page={currentPage}", Method.Get);
                    var response = _apiClient.ExecuteRequest(request);
                    var jsonResponse = JObject.Parse(response.Content);
                    totalPages = (int)jsonResponse["total_pages"];
                }

                currentPage++;
            } while (currentPage <= totalPages);

            return allUsers;
        }

        private List<JObject> GetUsersFromPage(int pageNumber)
        {
            var request = RestClientHelper.CreateRequest($"api/users?page={pageNumber}", Method.Get);
            var response = _apiClient.ExecuteRequest(request);

            if (!response.IsSuccessful)
                throw new Exception($"API call failed on page {pageNumber}");

            var jsonResponse = JObject.Parse(response.Content);
            var users = jsonResponse["data"];

            if (users == null)
                throw new Exception($"No users found on page {pageNumber}");

            return users.Select(user => (JObject)user).ToList();
        }

        public List<JObject> SortUsersByFirstName(List<JObject> users)
        {
            return users.OrderBy(user => (string)user["first_name"]).ToList();
        }

        public void PrintUsers(List<JObject> users)
        {
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user["id"]}, Email: {user["email"]}, First Name: {user["first_name"]}");
            }
        }
    }
}
