using APIRestSharp.APIClient;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace APIRestSharp.Operations
{
    public class UserApiOperations
    {
        private readonly ApiClient _apiClient;

        // Constructor that initializes the ApiClient
        public UserApiOperations(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // Method to get all users from multiple pages
        public List<JObject> GetAllUsers()
        {
            var allUsers = new List<JObject>();
            int currentPage = 1;
            int totalPages = 0;

            do
            {
                // Get users for the current page
                var users = GetUsersFromPage(currentPage);
                allUsers.AddRange(users);

                // If we are on the first page, fetch total pages count
                if (currentPage == 1)
                {
                    var request = new RestRequest($"api/users?page={currentPage}", Method.Get);
                    var response = _apiClient.ExecuteRequest(request);
                    var jsonResponse = JObject.Parse(response.Content);
                    totalPages = (int)jsonResponse["total_pages"];
                }

                currentPage++;
            } while (currentPage <= totalPages); // Continue fetching users until we have data from all pages

            return allUsers; // Return the collected list of all users
        }

        // Helper method to fetch users from a specific page
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

            // Convert each user to JObject and return the list
            return users.Select(user => (JObject)user).ToList();
        }

        // Method to get details of a specific user by their ID
        public JObject GetUserDetails(int userId)
        {
            var request = RestClientHelper.CreateRequest($"api/users/{userId}", Method.Get);
            var response = _apiClient.ExecuteRequest(request);

            if (!response.IsSuccessful)
            {
                var errorMessage = $"API call failed for user ID {userId}";
                Console.WriteLine(errorMessage);  
                throw new Exception(errorMessage);  
            }

            var jsonResponse = JObject.Parse(response.Content);
            var user = jsonResponse["data"];

            if (user == null)
                throw new Exception($"User with ID {userId} not found");

            // Return the user details as JObject
            return user as JObject;
        }

        // Helper method to assert the details of the extracted user
        public void AssertUserDetails(JObject user, int expectedId, string expectedEmail, string expectedFirstName)
        {
            if (user == null)
                throw new Exception("User data is null");

            // Assert that the user ID matches the expected ID
            if ((int)user["id"] != expectedId)
                throw new Exception($"Expected user ID {expectedId}, but got {(int)user["id"]}");

            // Assert that the user's email matches the expected email
            if ((string)user["email"] != expectedEmail)
                throw new Exception($"Expected email {expectedEmail}, but got {(string)user["email"]}");

            // Assert that the user's first name matches the expected first name
            if ((string)user["first_name"] != expectedFirstName)
                throw new Exception($"Expected first name {expectedFirstName}, but got {(string)user["first_name"]}");
        }

        // Method to sort the list of users by their first name
        public List<JObject> SortUsersByFirstName(List<JObject> users)
        {
            // Order the users by first name and return the sorted list
            return users.OrderBy(user => (string)user["first_name"]).ToList();
        }

        // Method to print the details of each user
        public void PrintUsers(List<JObject> users)
        {
            foreach (var user in users)
            {
                // Print the ID, email, and first name of each user
                Console.WriteLine($"ID: {user["id"]}, Email: {user["email"]}, First Name: {user["first_name"]}");
            }
        }
    }
}
