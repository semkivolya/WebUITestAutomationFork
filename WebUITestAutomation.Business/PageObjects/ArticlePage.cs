using OpenQA.Selenium;
using WebUITestAutomation.Core;

namespace WebUITestAutomation.Business.PageObjects
{
    public class ArticlePage : BasePage
    {
        private readonly By _articleTitleBy = By.CssSelector(".header_and_download span span");
        public ArticlePage(IWebDriver driver) : base(driver)
        {
        }

        public string GetArticleTitle()
        {
            var titleElement = driver.FindElementWithWait(_articleTitleBy);
            return titleElement.Text.Trim();
        }
    }
}