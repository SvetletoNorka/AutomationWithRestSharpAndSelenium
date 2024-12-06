using AventStack.ExtentReports;
using OnlineShoping.Drivers;
using OnlineShoping.Reporting;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace OnlineShoping.StepDefinitions
{
    [Binding]
    public class Login
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverExtensions _webDriverExtensions;
        private readonly ConfigurationReader config = new ConfigurationReader();

        // Locators
        private readonly By username = By.Id("user-name");
        private readonly By password = By.Id("password");
        private readonly By login = By.Id("login-button");

        public Login()
        {
            _driver = WebDriverController.Driver;
            _webDriverExtensions = new WebDriverExtensions(_driver, TimeSpan.FromSeconds(10));
        }

        [Given(@"I log in with the standard user")]
        public void GivenILogInWithTheStandardUser()
        {
            try
            {
                _driver.Navigate().GoToUrl(config.Url);
                Reporter.LogToReport(Status.Pass, $"Navigated to URL: {config.Url}");
                LoginWithUser(config.StandardUser);
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to navigate to the login page or log in: {ex.Message}");
                throw;
            }
        }

        public void LoginWithUser(string user)
        {
            try
            {
                _webDriverExtensions.EnterTextInField(username, user, 10);
                Reporter.LogToReport(Status.Info, $"Entered username: {user}");

                _webDriverExtensions.EnterTextInField(password, config.Password, 10);
                Reporter.LogToReport(Status.Info, "Entered password successfully.");

                _webDriverExtensions.FindAndClick(login);
                Reporter.LogToReport(Status.Info, "Clicked the login button successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Error occurred during login: {ex.Message}");
                throw;
            }
        }
    }
}
