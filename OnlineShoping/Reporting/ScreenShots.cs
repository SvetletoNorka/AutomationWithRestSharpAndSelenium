using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace OnlineShoping.Reporting
{
    public class ScreenShots
    {
        private readonly IWebDriver _driver;

        // Constructor to initialize the driver
        public ScreenShots(IWebDriver driver)
        {
            _driver = driver;
        }

        public string CaptureScreenshot(string fileName)
        {
            try
            {
                // Create directory for screenshots if it doesn't exist
                string screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                if (!Directory.Exists(screenshotsDir))
                {
                    Directory.CreateDirectory(screenshotsDir);
                }

                // Capture the screenshot
                ITakesScreenshot screenshotDriver = (ITakesScreenshot)_driver;
                Screenshot screenshot = screenshotDriver.GetScreenshot();

                // Save screenshot to file
                string filePath = Path.Combine(screenshotsDir, $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
                Reporter.LogToReport(Status.Error, $"Failed to capture screenshot: {ex.Message}");
                return null;
            }
        }
    }
}
