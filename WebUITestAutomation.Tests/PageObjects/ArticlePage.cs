using OpenQA.Selenium;

namespace WebUITestAutomation.Tests.PageObjects
{
    public class ArticlePage : BasePage
    {
        private readonly By _articleTitleBy = By.CssSelector(".header_and_download span span");
        public ArticlePage(IWebDriver driver) : base(driver)
        {
        }

        public string GetArticleTitle()
        {
            var titleElement = FindElement(_articleTitleBy);
            return titleElement.Text.Trim();
        }
    }
}