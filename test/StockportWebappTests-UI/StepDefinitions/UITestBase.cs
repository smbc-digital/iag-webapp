using Coypu;

namespace dts_frontend_tests_ui.StepDefinitions
{
    public abstract class UiTestBase
    {
        public static BrowserSession BrowserSession = BrowserConfiguration.BrowserSession;
        public static MockConfiguration MockConfiguration;
        public const bool IsRecordMode = false;

        protected UiTestBase()
        {
            MockConfiguration = new MockConfiguration(IsRecordMode);
        }
    }
}
