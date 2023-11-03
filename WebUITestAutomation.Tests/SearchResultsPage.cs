using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace WebUITestAutomation.Tests
{
    public class SearchResultsPage : BasePage
    {
        private readonly By _searchResultItemTextBy = By.CssSelector(".search-results__item p");

        public SearchResultsPage(IWebDriver driver, IConfiguration configuration) : base(driver, configuration)
        {
        }

        public List<string> GetSearchResultsText()
        {
            return FindElements(_searchResultItemTextBy).Select(p => p.Text).ToList();
        }
    }
}
