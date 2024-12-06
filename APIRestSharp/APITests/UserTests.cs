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
using Newtonsoft.Json.Linq;

namespace APIRestSharp.APITests
{
    [TestFixture]
    public class ApiUsersOperationsTests
    {
        private ApiClient _apiClient;
        private UserApiOperations _userApiOperations;

        [SetUp]
        public void Setup()
        {
            // Load the configuration from appsettings.json
            var configuration = ConfigurationReader.GetConfiguration();
            string baseUrl = configuration["ApiSettings:BaseUrl"];

            _apiClient = new ApiClient(baseUrl);
            _userApiOperations = new UserApiOperations(_apiClient);
        }

        [Test]
        public void Test_ExtractAndSortAllUsers()
        {
            var allUsers = _userApiOperations.GetAllUsers();  // Get users from API
            var sortedUsers = _userApiOperations.SortUsersByFirstName(allUsers);  // Sort users by First Name
            _userApiOperations.PrintUsers(sortedUsers);  // Print sorted users to console
        }

        [Test]
        public void Test_GetUserDetails_ValidUserId()
        {
            int userId = 2;  // Valid user ID
            var user = _userApiOperations.GetUserDetails(userId);

            _userApiOperations.AssertUserDetails(user, userId, "janet.weaver@reqres.in", "Janet");

            _userApiOperations.PrintUsers(new List<JObject> { user });
        }

        [Test]
        public void Test_GetUserDetails_InvalidUserId()
        {
            int userId = 999;  // Invalid user Id

            Assert.Throws<Exception>(() =>
            {
                _userApiOperations.GetUserDetails(userId);
            });
        }
    }
}
