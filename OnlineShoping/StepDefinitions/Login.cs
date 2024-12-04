using OnlineShoping.Drivers;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace OnlineShoping.StepDefinitions
{
    [Binding]
    public class Login 
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverExtensions _webDriverExtensions;
        ConfigurationReader config = new ConfigurationReader();

        By username = By.Id("user-name");
        By password = By.Id("password");
        By login = By.Id("login-button");

        public Login()
        {
            _driver = WebDriverController.Driver;
            _webDriverExtensions = new WebDriverExtensions(_driver, TimeSpan.FromSeconds(10));
        }

        [Given(@"I log in with the standard user")]
        public void GivenILogInWithTheStandardUser()
        {
            _driver.Navigate().GoToUrl(config.Url);
            LoginWithUser(config.StandardUser);

        }

        public void LoginWithUser(string user)
        {
            _webDriverExtensions.EnterTextInField(username, user, 10);
            _webDriverExtensions.EnterTextInField(password, config.Password, 10);
            _webDriverExtensions.FindAndClick(login);
        }
    }
}
