using AventStack.ExtentReports;
using OnlineShoping.Drivers;
using OnlineShoping.Reporting;
using TechTalk.SpecFlow;

namespace OnlineShoping.Hooks
{
    [Binding]
    public class WebDriverHooks
    {
        private static readonly string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "TestResults", $"ExtentReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");

        [BeforeTestRun]
        public static void InitializeReport()
        {
            Reporter.SetUpExtentReport("Online Shopping Tests", "Automation Test Results", reportPath);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            WebDriverController.StartDriver();
            Reporter.CreateTest(ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var scenarioContext = ScenarioContext.Current;

            if (scenarioContext.TestError != null)
            {
                Reporter.LogToReport(Status.Fail, $"Test failed: {scenarioContext.TestError.Message}");
            }
            else
            {
                Reporter.LogToReport(Status.Pass, "Test passed successfully");
            }

            WebDriverController.Quit();
        }

        [AfterTestRun]
        public static void FinalizeReport()
        {
            Reporter.FlushReport();
        }
    }
}
