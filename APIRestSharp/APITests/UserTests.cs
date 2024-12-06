/************ Scenarios
 
1. List available users
GET /api/users?page=1
Execute one or many JSON Response Assertions
Extract single user details (Id, Email)
(Optional) Extract all users, sort them by First Name alphabetically. Print sorted collection.

2. Get extracted user details
GET /api/users/{USER_ID}
Execute one or many JSON Response Assertions

3. Try to get details of user that doesn't exist
GET /api/users/{USER_ID}
Execute one or many Assertions

4. Create UNIQUE new user
POST /api/users
Execute one or many JSON Response Assertions

5. Delete newly created user
DELETE /api/users/{USER_ID}
Execute one or many Assertions

6. Parameterize base URL
 
 ********************/

using APIRestSharp.APIClient;
using APIRestSharp.Operations;
using APIRestSharp.Reporting;
using Newtonsoft.Json.Linq;

namespace APIRestSharp.APITests
{
    [TestFixture]
    public class ApiUsersOperationsTests
    {
        private ApiClient _apiClient;
        private UserApiOperations _userApiOperations;
        private static readonly string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "APITestsResults", $"APITestsResults_{DateTime.Now:yyyyMMdd_HHmmss}.html");

        [SetUp]
        public void Setup()
        {
            Reporter.SetUpExtentReport("API Tests", "Automation Test Results", reportPath);

            // Load the configuration from appsettings.json
            var configuration = ConfigurationReader.GetConfiguration();
            string baseUrl = configuration["ApiSettings:BaseUrl"];

            _apiClient = new ApiClient(baseUrl);
            _userApiOperations = new UserApiOperations(_apiClient);
        }

        [Test]
        public void Test_ExtractAndSortAllUsers()
        {
            Reporter.CreateTest("Test_ExtractAndSortAllUsers");
            var allUsers = _userApiOperations.GetAllUsers();  // Get users from API
            var sortedUsers = _userApiOperations.SortUsersByFirstName(allUsers);  // Sort users by First Name
            _userApiOperations.PrintUsers(sortedUsers);  // Print sorted users to console
        }

        [Test]
        public void Test_GetUserDetails_ValidUserId()
        {
            Reporter.CreateTest("Test_GetUserDetails_ValidUserId");

            int userId = 2;  // Valid user ID
            var user = _userApiOperations.GetUserDetails(userId);

            _userApiOperations.AssertUserDetails(user, userId, "janet.weaver@reqres.in", "Janet");

            _userApiOperations.PrintUsers(new List<JObject> { user });
        }

        [Test]
        public void Test_GetUserDetails_InvalidUserId()
        {
            Reporter.CreateTest("Test_GetUserDetails_InvalidUserId");

            int userId = 999;  // Invalid user Id

            Assert.Throws<Exception>(() =>
            {
                _userApiOperations.GetUserDetails(userId);
            });
        }

        // Flush the report after all tests are completed
        [TearDown]
        public void TearDown()
        {
            Reporter.FlushReport(); // This writes all collected data into the report file
        }
    }
}
