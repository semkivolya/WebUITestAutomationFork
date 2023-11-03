using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace WebUITestAutomation.Tests
{
    public class ArticlePage : BasePage
    {
        private readonly By _articleTitleBy = By.CssSelector(".header_and_download span span");
        public ArticlePage(IWebDriver driver, IConfiguration configuration) : base(driver, configuration)
        {
        }

        public string GetArticleTitle()
        {
            var titleElement = FindElement(_articleTitleBy);
            return titleElement.Text.Trim();
        }
    }
}