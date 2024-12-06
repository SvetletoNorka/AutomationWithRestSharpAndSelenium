using APIRestSharp.APIClient;
using Newtonsoft.Json.Linq;
using RestSharp;
using AventStack.ExtentReports;
using APIRestSharp.Reporting; 

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
                // Log the request to get users from the current page
                Reporter.LogToReport(Status.Info, $"Fetching users from page {currentPage}");

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
                    Reporter.LogToReport(Status.Info, $"Total pages available: {totalPages}");
                }

                currentPage++;
            } while (currentPage <= totalPages); // Continue fetching users until we have data from all pages

            // Return the collected list of all users
            Reporter.LogToReport(Status.Pass, $"Successfully retrieved all users:\n{allUsers}");
            return allUsers;
        }

        // Helper method to fetch users from a specific page
        private List<JObject> GetUsersFromPage(int pageNumber)
        {
            var request = RestClientHelper.CreateRequest($"api/users?page={pageNumber}", Method.Get);
            var response = _apiClient.ExecuteRequest(request);

            if (!response.IsSuccessful)
            {
                var errorMessage = $"API call failed on page {pageNumber}";
                Reporter.LogToReport(Status.Fail, errorMessage); // Log failure
                throw new Exception(errorMessage);
            }

            var jsonResponse = JObject.Parse(response.Content);
            var users = jsonResponse["data"];

            if (users == null)
            {
                var errorMessage = $"No users found on page {pageNumber}";
                Reporter.LogToReport(Status.Fail, errorMessage); // Log failure
                throw new Exception(errorMessage);
            }

            // Log success and return users
            Reporter.LogToReport(Status.Pass, $"Successfully retrieved users from page {pageNumber}");
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
                Reporter.LogToReport(Status.Info, errorMessage); // Log failure
                Console.WriteLine(errorMessage);  // Log to console
                throw new Exception(errorMessage);
            }

            var jsonResponse = JObject.Parse(response.Content);
            var user = jsonResponse["data"];

            if (user == null)
            {
                var errorMessage = $"User with ID {userId} not found";
                Reporter.LogToReport(Status.Info, errorMessage); // Log failure
                throw new Exception(errorMessage);
            }

            // Log success and return user details
            Reporter.LogToReport(Status.Pass, "Successfully retrieved details for user ID");
            return user as JObject;
        }

        // Sends a POST request to create a new user
        public JObject CreateUser(string name, string job)
        {
            var request = RestClientHelper.CreateRequest("api/users", Method.Post);

            var userPayload = new
            {
                name = name,
                job = job
            };

            request.AddJsonBody(userPayload);

            var response = _apiClient.ExecuteRequest(request);

            // Check if the response status is HTTP 201 Created
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                var errorMessage = $"Failed to create user. HTTP Status: {response.StatusCode}, Response: {response.Content}";
                Reporter.LogToReport(Status.Fail, errorMessage);
                Console.WriteLine(errorMessage);
                throw new Exception(errorMessage);
            }

            Reporter.LogToReport(Status.Pass, $"User successfully created. HTTP 201 Created. Response: {response.Content}");
            return JObject.Parse(response.Content);
        }

        // Validates the details of a created user against expected values.
        public void ValidateCreatedUserResponse(JObject responseJson, string expectedName, string expectedJob)
        {
            if (responseJson["id"] == null || responseJson["createdAt"] == null)
            {
                var errorMessage = "Response JSON does not contain required fields (id or createdAt).";
                Reporter.LogToReport(Status.Fail, errorMessage);
                throw new Exception(errorMessage);
            }

            Assert.AreEqual(expectedName, responseJson["name"]?.ToString(), "User name does not match.");
            Assert.AreEqual(expectedJob, responseJson["job"]?.ToString(), "User job does not match.");

            Reporter.LogToReport(
                Status.Pass,
                $"Validated created user: ID = {responseJson["id"]}, Name = {responseJson["name"]}, Job = {responseJson["job"]}, CreatedAt = {responseJson["createdAt"]}"
            );
        }

        // Method to delete a user by ID
        public void DeleteUser(int userId)
        {
            var request = RestClientHelper.CreateRequest($"api/users/{userId}", Method.Delete);
            var response = _apiClient.ExecuteRequest(request);

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var errorMessage = $"Failed to delete user with ID {userId}. HTTP Status: {response.StatusCode}, Response: {response.Content}";
                Reporter.LogToReport(Status.Fail, errorMessage);
                Console.WriteLine(errorMessage);
                throw new Exception(errorMessage);
            }

            Reporter.LogToReport(Status.Pass, $"User with ID {userId} successfully deleted. HTTP 204 No Content.");
        }

        // Method to validate that a user does not exist after deletion
        public void ValidateDeletedUserResponse(int userId)
        {
            var request = RestClientHelper.CreateRequest($"api/users/{userId}", Method.Get);
            var response = _apiClient.ExecuteRequest(request);

            // Expect a 404 status when trying to retrieve the deleted user
            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var errorMessage = $"User with ID {userId} still exists. Expected 404 Not Found but got {response.StatusCode}.";
                Reporter.LogToReport(Status.Fail, errorMessage);
                Console.WriteLine(errorMessage);
                throw new Exception(errorMessage);
            }

            Reporter.LogToReport(Status.Pass, $"User with ID {userId} successfully deleted and no longer exists.");
        }

        // Helper method to assert the details of the extracted user
        public void AssertUserDetails(JObject user, int expectedId, string expectedEmail, string expectedFirstName)
        {
            if (user == null)
            {
                var errorMessage = "User data is null";
                Reporter.LogToReport(Status.Fail, errorMessage); // Log failure
                throw new Exception(errorMessage);
            }

            // Assert that the user ID matches the expected ID
            if ((int)user["id"] != expectedId)
            {
                var errorMessage = $"Expected user ID {expectedId}, but got {(int)user["id"]}";
                Reporter.LogToReport(Status.Fail, errorMessage); // Log failure
                throw new Exception(errorMessage);
            }

            // Assert that the user's email matches the expected email
            if ((string)user["email"] != expectedEmail)
            {
                var errorMessage = $"Expected email {expectedEmail}, but got {(string)user["email"]}";
                Reporter.LogToReport(Status.Fail, errorMessage); // Log failure
                throw new Exception(errorMessage);
            }

            // Assert that the user's first name matches the expected first name
            if ((string)user["first_name"] != expectedFirstName)
            {
                var errorMessage = $"Expected first name {expectedFirstName}, but got {(string)user["first_name"]}";
                Reporter.LogToReport(Status.Fail, errorMessage); // Log failure
                throw new Exception(errorMessage);
            }

            // Log success after all assertions pass
            Reporter.LogToReport(Status.Pass, "User details are as expected.");
        }

        // Method to sort the list of users by their first name
        public List<JObject> SortUsersByFirstName(List<JObject> users)
        {
            // Order the users by first name and return the sorted list
            var sortedUsers = users.OrderBy(user => (string)user["first_name"]).ToList();
            Reporter.LogToReport(Status.Pass, "Successfully sorted users by first name.");
            return sortedUsers;
        }

        // Method to print the details of each user
        public void PrintUsers(List<JObject> users)
        {
            // Iterate through each user and print their details
            foreach (var user in users)
            {
                // Include Last Name in the user details
                string userDetails = $"ID: {user["id"]}, Email: {user["email"]}, First Name: {user["first_name"]}, Last Name: {user["last_name"]}";

                // Print to the console
                Console.WriteLine(userDetails);

                // Log to the ExtentReport as well
                Reporter.LogToReport(Status.Info, userDetails); // Logs user details into the report
            }

            // Log a message in the report indicating that the user details were successfully printed
            Reporter.LogToReport(Status.Pass, "Successfully printed all user details (ID, Email, First Name, Last Name) to console and report.");
        }

    }
}
