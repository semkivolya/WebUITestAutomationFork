using OpenQA.Selenium;

namespace WebUITestAutomation.Tests.PageObjects
{
    public class SearchResultsPage : BasePage
    {
        private readonly By _searchResultItemTextBy = By.CssSelector(".search-results__item p");

        public SearchResultsPage(IWebDriver driver) : base(driver)
        {
        }

        public List<string> GetSearchResultsText()
        {
            return FindElements(_searchResultItemTextBy).Select(p => p.Text).ToList();
        }
    }
}
