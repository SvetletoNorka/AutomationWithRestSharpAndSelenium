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

namespace APIRestSharp.Tests
{
    [TestFixture]
    public class ApiUsersOperationsTests
    {
        private ApiClient _apiClient;
        private ApiUsersOperations _userService;

        [SetUp]
        public void Setup()
        {
            // Load the configuration from appsettings.json
            var configuration = ConfigurationReader.GetConfiguration();
            string baseUrl = configuration["ApiSettings:BaseUrl"];

            _apiClient = new ApiClient(baseUrl);
            _userService = new ApiUsersOperations(_apiClient);  
        }

        [Test]
        public void Test_ExtractAndSortAllUsers()
        {
            var allUsers = _userService.GetAllUsers();  // Get users from API
            var sortedUsers = _userService.SortUsersByFirstName(allUsers);  // Sort users by First Name
            _userService.PrintUsers(sortedUsers);  // Print sorted users to console
        }
    }
}
