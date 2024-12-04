using System.Diagnostics;
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
        private readonly By addTocart = By.CssSelector("button[name^='add-to-cart']");
        private readonly By cart = By.XPath("//*[@data-test='shopping-cart-link']");
        private readonly By continueShopping = By.Id("continue-shopping");
        private readonly By checkout = By.Id("checkout");
        private readonly By cartLnk = By.CssSelector("a.shopping_cart_link");
        private readonly By cartBadge = By.CssSelector("span.shopping_cart_badge");
        private readonly By burgerMenu = By.Id("react-burger-menu-btn");
        private readonly By logOut = By.Id("logout_sidebar_link");
        private readonly By sortingMenu = By.XPath("//*[@data-test='product-sort-container']");
        private readonly By prices = By.CssSelector(".inventory_item .inventory_item_price");

        public Products()
        {
            _driver = WebDriverController.Driver;
            _webDriverExtensions = new WebDriverExtensions(_driver, TimeSpan.FromSeconds(10));
        }

        [When(@"I add ""([^""]*)"" item in the cart")]
        public void WhenIAddItemInTheCart(string index)
        {
            AddItemToCart(index);
        }

        [Then(@"I open the cart")]
        public void ThenIOpenTheCart()
        {
            _webDriverExtensions.FindAndClick(cart);
        }

        [When(@"I remove ""([^""]*)"" item from cart")]
        public void WhenIRemoveItemFromCart(string index)
        {
            if (index == "first")
                RemoveItemFromCart(firstItemName);
            if (index == "last")
                RemoveItemFromCart(lastItemName);
            if (index == "previous of last")
                RemoveItemFromCart(previousOflastItemName);
        }

        [Then(@"I continue with shoping")]
        public void ThenIContinueWithShoping()
        {
            _webDriverExtensions.FindAndClick(continueShopping);
        }

        [Then(@"I verify ""([^""]*)"" item is in the cart")]
        public void ThenIVerifyFirstItemIsInTheCart(string index)
        {
            if(index == "first")
                VerifyItemInCart(firstItemName);
            if (index == "last")
                VerifyItemInCart(lastItemName);
            if (index == "previous of last")
                VerifyItemInCart(previousOflastItemName);
        }

        [When(@"I go to checkout")]
        public void WhenIGoToCheckout()
        {
            _webDriverExtensions.FindAndClick(checkout);
        }

        [Then(@"I verify the cart is empty")]
        public void ThenIVerifyTheCartIsEmpty()
        {
            VerifyCartIsEmpty();
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

    // Helper method to add an item to the cart by index
    private void AddItemToCart(string index)
        {
            var inventoryItems = _driver.FindElements(productList);

            if (inventoryItems.Count > 0)
            {
                IWebElement item = null;  // Declare item variable outside of condition

                // Check which item to add to cart
                if (index == "first")
                {
                    item = inventoryItems[0]; // Get the first item
                    firstItemName = _webDriverExtensions.ReadItemName(item);
                }
                else if (index == "last")
                {
                    item = inventoryItems[inventoryItems.Count - 1]; // Get the last item
                    lastItemName = _webDriverExtensions.ReadItemName(item);
                }
                else if (index == "previous of last" && inventoryItems.Count > 1)
                {
                    item = inventoryItems[inventoryItems.Count - 2]; // Get the second-to-last item
                    previousOflastItemName = _webDriverExtensions.ReadItemName(item);
                }

                // If the item is not null, click the add to cart button
                if (item != null)
                {
                    var addToCartButton = item.FindElement(addTocart);
                    addToCartButton.Click(); // Add item to cart
                }
            }
        }

        // Helper method to verify an item in the cart by item name
        private void VerifyItemInCart(string itemName)
        {
            By itemCartXpath = By.XPath($"//div[@data-test='inventory-item-name' and text()='{itemName}']");
            _webDriverExtensions.AssertElementIsDisplayed(itemCartXpath); // Verify item is in the cart
        }

        // Helper method to remove an item from the cart by name
        private void RemoveItemFromCart(string itemName)
        {
            By itemCartXpath = By.XPath($"//div[@class='cart_item_label' and .//div[@class='inventory_item_name' and text()='{itemName}']]//button[normalize-space()='Remove']");
            _webDriverExtensions.FindAndClick(itemCartXpath);
        }

        // Helper method to verify that the cart is empty
        private void VerifyCartIsEmpty()
        {
            // Locate the shopping cart link element
            var cartLink = _driver.FindElement(cartLnk);

            // Check if the span.shopping_cart_badge is present
            bool isBadgePresent = cartLink.FindElements(cartBadge).Count == 0;

            Assert.IsTrue(isBadgePresent);
        }
    }
}
