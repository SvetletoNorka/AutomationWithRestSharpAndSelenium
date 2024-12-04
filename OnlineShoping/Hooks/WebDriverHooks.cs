using OnlineShoping.Drivers;
using TechTalk.SpecFlow;

namespace OnlineShoping.Hooks
{
    [Binding]
    public class WebDriverHooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            WebDriverController.StartDriver();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            WebDriverController.Quit();
        }
    }
}
