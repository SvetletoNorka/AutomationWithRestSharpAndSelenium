using AventStack.ExtentReports;
using NUnit.Framework;
using OnlineShoping.Reporting;
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
            try
            {
                var element = _wait.Until(drv =>
                {
                    var elem = drv.FindElement(by);
                    return (elem.Displayed && elem.Enabled) ? elem : null;
                });

                element.Click();
                Reporter.LogToReport(Status.Pass, $"Clicked the element located by {by} successfully.");
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, $"Failed to click the element located by {by}: {ex.Message}");
                throw;
            }
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
                Reporter.LogToReport(Status.Pass, $"Verified that the element located by {by} is displayed.");
            }
            catch (WebDriverTimeoutException)
            {
                Reporter.LogToReport(Status.Fail, $"The element located by {by} was not visible within {timeoutInSeconds} seconds.");
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
                Reporter.LogToReport(Status.Pass, $"Entered text '{text}' in the field located by {by} successfully.");
            }
            catch (NoSuchElementException)
            {
                Reporter.LogToReport(Status.Fail, $"Failed to find the element located by {by} to enter text '{text}'.");
                throw;
            }
            catch (WebDriverTimeoutException)
            {
                Reporter.LogToReport(Status.Fail, $"Element with selector {by} was not visible within {timeoutInSeconds} seconds to enter text '{text}'.");
                throw;
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

                Reporter.LogToReport(Status.Pass, $"Selected option '{optionText}' from the dropdown menu located by {menuLocator}.");
            }
            catch (WebDriverTimeoutException)
            {
                Reporter.LogToReport(Status.Fail, $"Menu with selector {menuLocator} was not visible within {timeoutInSeconds} seconds.");
                throw;
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, $"Failed to select option '{optionText}' from the menu located by {menuLocator}: {ex.Message}");
                throw;
            }
        }

        public string ReadItemName(IWebElement element)
        {
            try
            {
                string itemName = element.Text.Split(new[] { "\r\n" }, StringSplitOptions.None)[0];
                Reporter.LogToReport(Status.Pass, $"Read item name: '{itemName}'.");
                return itemName;
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, $"Failed to read item name: {ex.Message}");
                throw;
            }
        }
    }
}
