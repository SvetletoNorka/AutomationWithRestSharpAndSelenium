using OnlineShoping.Drivers;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace OnlineShoping.StepDefinitions
{
    [Binding]
    public class Checkout
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverExtensions _webDriverExtensions;

        // Locators
        private readonly By firstName = By.Id("first-name");
        private readonly By lastName = By.Id("last-name");
        private readonly By zipCode = By.Id("postal-code");
        private readonly By continueBtn = By.Id("continue");
        private readonly By finish = By.Id("finish");
        private readonly By checkoutComplete = By.XPath("//span[@class='title' and text()='Checkout: Complete!']");
        private readonly By backHome = By.Id("back-to-products");

        public Checkout()
        {
            _driver = WebDriverController.Driver;
            _webDriverExtensions = new WebDriverExtensions(_driver, TimeSpan.FromSeconds(10));
        }

        [Then(@"I fill first name = ""([^""]*)""")]
        public void ThenIFillFirstName(string name)
        {
            _webDriverExtensions.EnterTextInField(firstName, name);
        }

        [Then(@"I fill last name = ""([^""]*)""")]
        public void ThenIFillLastName(string name)
        {
            _webDriverExtensions.EnterTextInField(lastName, name);
        }

        [Then(@"I fill zip code = ""([^""]*)""")]
        public void ThenIFillZipCode(string zip)
        {
            _webDriverExtensions.EnterTextInField(zipCode, zip);
        }

        [When(@"I press continue")]
        public void WhenIPressContinue()
        {
            _webDriverExtensions.FindAndClick(continueBtn);
        }

        [Then(@"I finish the order")]
        public void ThenIFinishTheOrder()
        {
            _webDriverExtensions.FindAndClick(finish);
        }

        [Then(@"I verify the checkout is completed")]
        public void ThenIVerifyTheCheckoutIsCompleted()
        {
            _webDriverExtensions.AssertElementIsDisplayed(checkoutComplete);
        }

        [When(@"I press Back Home")]
        [Then(@"I press Back Home")]
        public void ThenIPressBackHome()
        {
            _webDriverExtensions.FindAndClick(backHome);
        }
    }
}
