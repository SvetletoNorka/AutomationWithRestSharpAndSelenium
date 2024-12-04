using OnlineShoping.Drivers;
using OpenQA.Selenium;
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
    }
}
