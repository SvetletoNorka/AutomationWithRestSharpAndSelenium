using AventStack.ExtentReports;
using NUnit.Framework;
using OnlineShoping.Drivers;
using OnlineShoping.Reporting;
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
        private readonly By checkout = By.Id("checkout");
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

        [When(@"I go to checkout")]
        public void WhenIGoToCheckout()
        {
            try
            {
                _webDriverExtensions.FindAndClick(checkout);
                Reporter.LogToReport(Status.Pass, "Navigated to the checkout page successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to navigate to the checkout page: {ex.Message}");
                throw;
            }
        }

        [Then(@"I fill first name = ""([^""]*)""")]
        public void ThenIFillFirstName(string name)
        {
            try
            {
                _webDriverExtensions.EnterTextInField(firstName, name);
                Reporter.LogToReport(Status.Pass, $"Entered first name: {name}");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to enter first name: {ex.Message}");
                throw;
            }
        }

        [Then(@"I fill last name = ""([^""]*)""")]
        public void ThenIFillLastName(string name)
        {
            try
            {
                _webDriverExtensions.EnterTextInField(lastName, name);
                Reporter.LogToReport(Status.Pass, $"Entered last name: {name}");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to enter last name: {ex.Message}");
                throw;
            }
        }

        [Then(@"I fill zip code = ""([^""]*)""")]
        public void ThenIFillZipCode(string zip)
        {
            try
            {
                _webDriverExtensions.EnterTextInField(zipCode, zip);
                Reporter.LogToReport(Status.Pass, $"Entered zip code: {zip}");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to enter zip code: {ex.Message}");
                throw;
            }
        }

        [When(@"I press continue")]
        public void WhenIPressContinue()
        {
            try
            {
                _webDriverExtensions.FindAndClick(continueBtn);
                Reporter.LogToReport(Status.Pass, "Clicked the 'Continue' button successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to click the 'Continue' button: {ex.Message}");
                throw;
            }
        }

        [Then(@"I finish the order")]
        public void ThenIFinishTheOrder()
        {
            try
            {
                _webDriverExtensions.FindAndClick(finish);
                Reporter.LogToReport(Status.Pass, "Finished the order successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to finish the order: {ex.Message}");
                throw;
            }
        }

        [Then(@"I verify the checkout is completed")]
        public void ThenIVerifyTheCheckoutIsCompleted()
        {
            try
            {
                _webDriverExtensions.AssertElementIsDisplayed(checkoutComplete);
                Reporter.LogToReport(Status.Pass, "Verified that the checkout is completed.");
            }
            catch (AssertionException ex)
            {
                Reporter.LogToReport(Status.Fail, "Checkout completion verification failed.");
                throw;
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"An error occurred during checkout verification: {ex.Message}");
                throw;
            }
        }

        [When(@"I press Back Home")]
        [Then(@"I press Back Home")]
        public void ThenIPressBackHome()
        {
            try
            {
                _webDriverExtensions.FindAndClick(backHome);
                Reporter.LogToReport(Status.Pass, "Clicked the 'Back Home' button successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to click the 'Back Home' button: {ex.Message}");
                throw;
            }
        }
    }
}
