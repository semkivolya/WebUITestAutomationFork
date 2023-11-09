using OpenQA.Selenium;
using TestAutomation.Core;

namespace TestAutomation.Business.UI.PageObjects
{
    public class SearchResultsPage : BasePage
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly By _searchResultItemTextBy = By.CssSelector(".search-results__item p");

        public SearchResultsPage(IWebDriver driver) : base(driver)
        {
        }

        public List<string> GetSearchResultsText()
        {
            return driver.FindElementsWithWait(_searchResultItemTextBy).Select(p => p.Text).ToList();
        }
    }
}
