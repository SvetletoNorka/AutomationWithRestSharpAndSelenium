using NUnit.Framework;
using OnlineShoping.Drivers;
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

        // Locators for product list and add-to-cart button
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
            _webDriverExtensions.FindAndClick(burgerMenu);
        }

        [Then(@"I logout from the system")]
        public void ThenILogoutFromTheSystem()
        {
            _webDriverExtensions.FindAndClick(logOut);
        }

        [When(@"I select sorting = ""([^""]*)""")]
        public void WhenISelectSorting(string sorting)
        {
            _webDriverExtensions.SelectOptionFromMenu(sortingMenu, sorting);
        }

        [Then(@"I verify that the sorting of items by descending price is correct")]
        public void ThenIVerifyThatTheSortingOfItemsByDescendingPriceIsCorrect()
        {
            // Wait loading of list of product
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElements(productList).Count > 0);

            // Gathering the prices
            List<decimal> productPrices = new List<decimal>();
            var priceElements = _driver.FindElements(prices);

            foreach (var priceElement in priceElements)
            {
                string priceText = priceElement.Text.Trim('$'); // Remove $ symbol
                decimal price = Convert.ToDecimal(priceText);
                productPrices.Add(price);
            }

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

            // Assert that the sorting is correct, display a message in case of failure 
            Assert.IsTrue(isSortedCorrectly, "The products are not sorted correctly by price in descending order.");

            Console.WriteLine("Price sorting works correctly.");
        }
    }
}
