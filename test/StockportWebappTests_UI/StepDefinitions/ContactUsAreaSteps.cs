using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "contactUsArea")]
    class ContactUsAreaSteps : UiTestBase
    {
        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "inset text":
                    result = BrowserSession.FindCss(".alert-insetText").Exists();
                    break;
                case "heading":
                    result = BrowserSession.FindCss("h1").Exists();
                    break;
                case "top three items":
                    result = BrowserSession.FindCss(".dashboard-button-container").Exists();
                    break;
                case "categories container":
                    result = BrowserSession.FindAllCss(".contact-us-category-container").Any();
                    break;
                case "contact us categories":
                    result = BrowserSession.FindAllCss(".contact-us-category").Any();
                    break;
                case "category header":
                    result = BrowserSession.FindAllCss(".category-header").Any();
                    break;
                case "category body":
                    result = BrowserSession.FindAllCss(".category-body").Any();
                    break;
            }
            Assert.True(result);
        }
    }
}
