using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OnlineShoping.Drivers
{
    public class WebDriverExtensions
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WebDriverExtensions(IWebDriver driver, TimeSpan waitTime)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, waitTime);
        }

        public void FindAndClick(By by)
        {
            // Wait for the element to be clickable
            var element = _wait.Until(drv =>
            {
                var elem = drv.FindElement(by);
                return (elem.Displayed && elem.Enabled) ? elem : null;
            });

            //Click the element
            element.Click();
        }

        public void AssertElementIsDisplayed(By by, int timeoutInSeconds = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));

                IWebElement element = wait.Until(driver =>
                {
                    try
                    {
                        var webElement = driver.FindElement(by);
                        return webElement.Displayed ? webElement : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });

                Assert.IsTrue(element != null && element.Displayed, $"The element located by {by} is not displayed on the page.");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"The element located by {by} was not visible within {timeoutInSeconds} seconds.");
            }
        }

        public void EnterTextInField(By by, string text, int timeoutInSeconds = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement inputField = wait.Until(driver => driver.FindElement(by));

                inputField.Clear();
                inputField.SendKeys(text);
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Element not found: {by}");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Element with selector {by} was not visible within {timeoutInSeconds} seconds.");
            }
        }

        public void SelectOptionFromMenu(By menuLocator, string optionText, int timeoutInSeconds = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement dropdownElement = wait.Until(driver => driver.FindElement(menuLocator));

                var selectElement = new SelectElement(dropdownElement);

                selectElement.SelectByText(optionText);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Menu with selector {menuLocator} was not visible within {timeoutInSeconds} seconds.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public string ReadItemName(IWebElement element)
        {
            string itemName = element.Text.Split(new[] { "\r\n" }, StringSplitOptions.None)[0];
            return itemName;
        }
    }
}
