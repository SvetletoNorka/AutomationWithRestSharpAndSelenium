using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace APIRestSharp.Reporting
{
    public static class Reporter
    {
        private static readonly ExtentReports _extentReports = new ExtentReports();
        private static ExtentSparkReporter _sparkReporter;
        private static ExtentTest _testCase;

        public static void SetUpExtentReport(string reportName, string documentTitle, string path)
        {
            try
            {
                _sparkReporter = new ExtentSparkReporter(path)
                {
                    Config =
            {
                Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark,
                DocumentTitle = documentTitle,
                ReportName = reportName
            }
                };

                // Attach SparkReporter to ExtentReports
                _extentReports.AttachReporter(_sparkReporter);
                Console.WriteLine($"Report initialized at: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while setting up the report: {ex.Message}");
            }
        }

        public static void CreateTest(string testName)
        {
            _testCase = _extentReports.CreateTest(testName);
        }

        public static void LogToReport(Status status, string message)
        {
            _testCase?.Log(status, message);
        }

        public static void TestStatus(Status status)
        {
            if (status == Status.Pass)
            {
                _testCase.Pass("Test is passed");
            }
            else
            {
                _testCase.Fail("Test is failed");
            }
        }

        public static void FlushReport()
        {
            _extentReports?.Flush();
        }
    }
}
