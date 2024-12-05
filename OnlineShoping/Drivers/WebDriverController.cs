using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace OnlineShoping.Drivers
{
    public static class WebDriverController
    {
        public static IWebDriver Driver { get; set; }

        public static void StartDriver()
        {
            var driverManager = new DriverManager();
            ConfigurationReader config = new ConfigurationReader();
            string browserType = config.Browser;
            bool isHeadless = config.IsHeadless;

            switch (browserType)
            {
                case "Chrome":
                    driverManager.SetUpDriver(new ChromeConfig());
                    ChromeOptions chromeOptions = new ChromeOptions();
                    if (isHeadless)
                    {
                        chromeOptions.AddArgument("--headless"); // Add headless argument for Chrome
                    }
                    Driver = new ChromeDriver(chromeOptions);
                    break;

                case "Firefox":
                    driverManager.SetUpDriver(new FirefoxConfig());
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    if (isHeadless)
                    {
                        firefoxOptions.AddArgument("--headless"); // Add headless argument for Firefox
                    }
                    Driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "Edge":
                    driverManager.SetUpDriver(new EdgeConfig());
                    EdgeOptions edgeOptions = new EdgeOptions();
                    if (isHeadless)
                    {
                        edgeOptions.AddArgument("--headless"); // Add headless argument for Edge
                    }
                    Driver = new EdgeDriver(edgeOptions);
                    break;

                case "IE":
                    driverManager.SetUpDriver(new InternetExplorerConfig());
                    Driver = new InternetExplorerDriver();
                    break;

                default:
                    throw new NotSupportedException($"Browser type '{browserType}' is not supported.");
            }

            Driver.Manage().Window.Maximize();
        }

        public static void Quit()
        {
            Driver?.Quit();
            Driver = null;
        }
    }
}
