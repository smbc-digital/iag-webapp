using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "news")]
    class NewsSteps : UiTestBase
    {
        [Then(@"I should not see the ""(.*)"" section")]
        public void ThenIShouldNotSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "alert warning":
                    result = BrowserSession.FindCss(".alert .alert-warning").Exists();
                    break;
            }
            Assert.False(result);
        }


        [When(@"I click the ""(.*)"" element")]
        public void WhenIClickTheElement(string name)
        {
            switch (name)
            {
                case "Category":
                    BrowserSession.FindAllCss("#category-filter h3").FirstOrDefault().Click();
                    break;
                case "News archive":
                    BrowserSession.FindAllCss("#news-archive h3").FirstOrDefault().Click();
                    break;
                case "close warning button":
                    BrowserSession.FindAllCss(".alert-close a").FirstOrDefault().Click();
                    break;
            }
        }

        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "news header":
                    result = BrowserSession.FindCss("h1").Exists();
                    break;
                case "alert infomation":
                    result = BrowserSession.FindCss(".alert-information").Exists();
                    break;
                case "alert warning":
                    result = BrowserSession.FindCss(".alert-warning").Exists();
                    break;
                case "alert error":
                    result = BrowserSession.FindCss(".alert-error").Exists();
                    break;
                case "refine by category":
                    result = BrowserSession.FindCss(".mobile-filter-heading.filter-title").Exists();
                    break;
                case "refine by news archive":
                    result = BrowserSession.FindCss("#news-archive").Exists();
                    break;
                case "start date":
                    result = BrowserSession.FindCss(".date-from input.datepicker").Exists();
                    break;
                case "end date":
                    result = BrowserSession.FindCss(".date-to input.datepicker").Exists();
                    break;
                case "Custom date":
                    result = BrowserSession.FindCss("#custom-filter-li").Exists();
                    break;
                case "category list":
                    result = BrowserSession.FindCss("#category-filter ul").Exists();
                    break;
                case "Email alerts":
                    result = BrowserSession.FindCss("a[href*='https://public.govdelivery.com/accounts/UKSMBC/subscriber/new']").Exists();
                    break;
                case "news articles":
                    result = BrowserSession.FindCss(".nav-card-news-list").Exists() && BrowserSession.FindAllCss(".nav-card-news-list li").Any();
                    break;
            }
            Assert.True(result);
        }
    }
}
