using AventStack.ExtentReports;
using NUnit.Framework;
using OnlineShoping.Drivers;
using OnlineShoping.Reporting;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace OnlineShoping.StepDefinitions
{
    [Binding]
    public class Cart
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverExtensions _webDriverExtensions;
        private string firstItemName;
        private string lastItemName;
        private string previousOflastItemName;

        // Locators for cart 
        private readonly By productList = By.CssSelector(".inventory_item");
        private readonly By addTocart = By.CssSelector("button[name^='add-to-cart']");
        private readonly By cart = By.XPath("//*[@data-test='shopping-cart-link']");
        private readonly By continueShopping = By.Id("continue-shopping");
        private readonly By cartLnk = By.CssSelector("a.shopping_cart_link");
        private readonly By cartBadge = By.CssSelector("span.shopping_cart_badge");

        public Cart()
        {
            _driver = WebDriverController.Driver;
            _webDriverExtensions = new WebDriverExtensions(_driver, TimeSpan.FromSeconds(10));
        }

        [When(@"I add ""(first|last|previous of last)"" item in the cart")]
        public void WhenIAddItemInTheCart(string index)
        {
            try
            {
                AddItemToCart(index);
                Reporter.LogToReport(Status.Pass, $"Successfully added the {index} item to the cart.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to add the {index} item to the cart: {ex.Message}");
                throw;
            }
        }

        [Then(@"I open the cart")]
        public void ThenIOpenTheCart()
        {
            try
            {
                _webDriverExtensions.FindAndClick(cart);
                Reporter.LogToReport(Status.Pass, "Opened the cart successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to open the cart: {ex.Message}");
                throw;
            }
        }

        [When(@"I remove ""([^""]*)"" item from cart")]
        public void WhenIRemoveItemFromCart(string index)
        {
            try
            {
                if (index == "first")
                    RemoveItemFromCart(firstItemName);
                if (index == "last")
                    RemoveItemFromCart(lastItemName);
                if (index == "previous of last")
                    RemoveItemFromCart(previousOflastItemName);

                Reporter.LogToReport(Status.Pass, $"Successfully removed the {index} item from the cart.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to remove the {index} item from the cart: {ex.Message}");
                throw;
            }
        }

        [Then(@"I continue with shoping")]
        public void ThenIContinueWithShoping()
        {
            try
            {
                _webDriverExtensions.FindAndClick(continueShopping);
                Reporter.LogToReport(Status.Pass, "Continued shopping successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"Failed to continue shopping: {ex.Message}");
                throw;
            }
        }

        [Then(@"I verify ""(first|last|previous of last)"" item is in the cart")]
        public void ThenIVerifyFirstItemIsInTheCart(string index)
        {
            try
            {
                if (index == "first")
                    VerifyItemInCart(firstItemName);
                if (index == "last")
                    VerifyItemInCart(lastItemName);
                if (index == "previous of last")
                    VerifyItemInCart(previousOflastItemName);

                Reporter.LogToReport(Status.Pass, $"Verified the {index} item is in the cart.");
            }
            catch (AssertionException ex)
            {
                Reporter.LogToReport(Status.Fail, $"Verification failed: The {index} item is not in the cart.");
                throw;
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"An error occurred during verification: {ex.Message}");
                throw;
            }
        }

        [Then(@"I verify the cart is empty")]
        public void ThenIVerifyTheCartIsEmpty()
        {
            try
            {
                VerifyCartIsEmpty();
                Reporter.LogToReport(Status.Pass, "Verified the cart is empty.");
            }
            catch (AssertionException ex)
            {
                Reporter.LogToReport(Status.Fail, "Verification failed: The cart is not empty.");
                throw;
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Error, $"An error occurred while verifying the cart: {ex.Message}");
                throw;
            }
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

            Assert.IsTrue(isBadgePresent, "The cart is not empty.");
        }
    }
}
