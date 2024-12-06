using AventStack.ExtentReports;
using NUnit.Framework;
using OnlineShoping.Drivers;
using OnlineShoping.Reporting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace OnlineShoping.StepDefinitions
{
    [Binding]
    public class Products
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverExtensions _webDriverExtensions;
        private string firstItemName;
        private string lastItemName;
        private string previousOflastItemName;

        // Locators for product list 
        private readonly By productList = By.CssSelector(".inventory_item");
        private readonly By burgerMenu = By.Id("react-burger-menu-btn");
        private readonly By logOut = By.Id("logout_sidebar_link");
        private readonly By sortingMenu = By.XPath("//*[@data-test='product-sort-container']");
        private readonly By prices = By.CssSelector(".inventory_item .inventory_item_price");

        public Products()
        {
            _driver = WebDriverController.Driver;
            _webDriverExtensions = new WebDriverExtensions(_driver, TimeSpan.FromSeconds(10));
        }

        [When(@"I press burger menu")]
        public void WhenIPressBurgerMenu()
        {
            try
            {
                _webDriverExtensions.FindAndClick(burgerMenu);
                Reporter.LogToReport(Status.Pass, "Clicked on the burger menu.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to click the burger menu: {ex.Message}");
                throw;
            }
        }

        [Then(@"I logout from the system")]
        public void ThenILogoutFromTheSystem()
        {
            try
            {
                _webDriverExtensions.FindAndClick(logOut);
                Reporter.LogToReport(Status.Pass, "Logged out successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to log out: {ex.Message}");
                throw;
            }
        }

        [When(@"I select sorting = ""([^""]*)""")]
        public void WhenISelectSorting(string sorting)
        {
            try
            {
                _webDriverExtensions.SelectOptionFromMenu(sortingMenu, sorting);
                Reporter.LogToReport(Status.Info, $"Selected sorting option: {sorting}");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to select sorting option '{sorting}': {ex.Message}");
                throw;
            }
        }

        [Then(@"I verify that the sorting of items by descending price is correct")]
        public void ThenIVerifyThatTheSortingOfItemsByDescendingPriceIsCorrect()
        {
            try
            {
                // Wait for loading of the product list
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                wait.Until(driver => driver.FindElements(productList).Count > 0);
                Reporter.LogToReport(Status.Info, "Product list loaded successfully.");

                // Gather the prices
                List<decimal> productPrices = new List<decimal>();
                var priceElements = _driver.FindElements(prices);

                foreach (var priceElement in priceElements)
                {
                    string priceText = priceElement.Text.Trim('$'); // Remove $ symbol
                    decimal price = Convert.ToDecimal(priceText);
                    productPrices.Add(price);
                }
                Reporter.LogToReport(Status.Info, $"Captured product prices: {string.Join(", ", productPrices)}");

                // Sort the prices in descending order
                var sortedPrices = productPrices.OrderByDescending(p => p).ToList();

                // Assert: Check if the prices are correctly sorted
                bool isSortedCorrectly = true;
                for (int i = 0; i < productPrices.Count; i++)
                {
                    if (productPrices[i] != sortedPrices[i])
                    {
                        isSortedCorrectly = false;
                        break;
                    }
                }

                Assert.IsTrue(isSortedCorrectly, "The products are not sorted correctly by price in descending order.");
                Reporter.LogToReport(Status.Pass, "Products are sorted correctly by price in descending order.");
            }
            catch (AssertionException ex)
            {
                Reporter.LogToReport(Status.Fail, $"Price sorting verification failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"An error occurred during sorting verification: {ex.Message}");
                throw;
            }
        }
    }
}
