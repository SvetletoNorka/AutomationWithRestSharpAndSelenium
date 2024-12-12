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
using APIRestSharp.Models;
using APIRestSharp.Operations;
using APIRestSharp.Reporting;
using AventStack.ExtentReports;
using Newtonsoft.Json.Linq;

namespace APIRestSharp.APITests
{
    [TestFixture]
    public class UserAPITests
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

            try
            {
                var allUsers = _userApiOperations.GetAllUsers();  // Get users from API
                var sortedUsers = _userApiOperations.SortUsersByFirstName(allUsers);  // Sort users by First Name
                _userApiOperations.PrintUsers(sortedUsers);  // Print sorted users to console

                Reporter.LogToReport(Status.Pass, "Test passed. The list of users is successfully sorted by first name.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, $"Test failed.");
                Assert.Fail($"Test failed. Exception message: {ex.Message}");
            }
        }

        [Test]
        public void Test_GetUserDetails_ValidUserId()
        {
            Reporter.CreateTest("Test_GetUserDetails_ValidUserId");

            int userId = 2;  // Valid user ID

            try
            {
                var user = _userApiOperations.GetUserDetails(userId);

                _userApiOperations.AssertUserDetails(user, userId, "janet.weaver@reqres.in", "Janet");

                _userApiOperations.PrintUsers(new List<User> { user });

                Reporter.LogToReport(Status.Pass, $"Test passed. User with ID: {userId} is successfully retrieved.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, $"Test failed.");
                Assert.Fail($"Test failed. Exception message: {ex.Message}");
            }
        }

        [Test]
        public void Test_GetUserDetails_InvalidUserId()
        {
            // Create a test in the Extent report
            Reporter.CreateTest("Test_GetUserDetails_InvalidUserId");

            int userId = 23;  // Invalid user Id
            string expectedErrorMessage = $"API call failed for user ID {userId}";

            try
            {
                _userApiOperations.GetUserDetails(userId);
                Reporter.LogToReport(Status.Fail, $"Test failed. User with ID: {userId} is found.");
                Assert.Fail($"Test failed. User with ID: {userId} is found.");

            }
            catch (Exception ex)
            {
                // Check if the exception message is what we expected
                if (ex.Message.Contains(expectedErrorMessage))
                {
                    // Log the success in the report
                    Reporter.LogToReport(Status.Pass, $"Test passed. Exception was thrown as expected: {ex.Message}");
                }
                else
                {
                    // Log failure if the exception message doesn't match the expected message
                    Reporter.LogToReport(Status.Fail, $"Test failed. Unexpected exception message: {ex.Message}");
                    Assert.Fail($"Unexpected exception message: {ex.Message}");
                }
            }
        }

        [Test]
        public void Test_CreateUniqueUser()
        {
            Reporter.CreateTest("Test_CreateUniqueUser");

            // Generate unique user data
            string uniqueName = $"User_{DateTime.Now:yyyyMMdd_HHmmss}";
            string job = "Software Developer";

            try
            {
                // Call CreateUser method
                var user = _userApiOperations.CreateUser(uniqueName, job);

                // Validate the response
                _userApiOperations.ValidateCreatedUserResponse(user, uniqueName, job);

                Reporter.LogToReport(Status.Pass, $"Test passed. User with unique name: {uniqueName} is successfully created.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, $"Test failed.");
                Assert.Fail($"Test failed. Exception message: {ex.Message}");
            }
        }

        [Test]
        public void Test_DeleteNewlyCreatedUser()
        {
            Reporter.CreateTest("Test_DeleteNewlyCreatedUser");

            // Generate unique user data
            string uniqueName = $"User_{DateTime.Now:yyyyMMdd_HHmmss}";
            string job = "Software Developer";

            try
            {
                var userResponseJson = _userApiOperations.CreateUser(uniqueName, job);

                // Extract user ID from the response for deletion
                int userId = int.Parse(userResponseJson.Id);

                // Delete the newly created user
                _userApiOperations.DeleteUser(userId);

                // Verify that the user no longer exists (404 Not Found)
                _userApiOperations.ValidateDeletedUserResponse(userId);

                Reporter.LogToReport(Status.Pass, $"Test passed. User with ID: {userId} is deleted successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, $"Test failed.");
                Assert.Fail($"Test failed. Exception message: {ex.Message}");
            }
        }

        // Flush the report after all tests are completed
        [TearDown]
        public void TearDown()
        {
            Reporter.FlushReport(); // This writes all collected data into the report file
        }
    }
}
