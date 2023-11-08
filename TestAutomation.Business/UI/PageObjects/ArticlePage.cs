using OpenQA.Selenium;
using TestAutomation.Core;

namespace TestAutomation.Business.UI.PageObjects
{
    public class ArticlePage : BasePage
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly By _articleTitleBy = By.CssSelector(".header_and_download span span");
        public ArticlePage(IWebDriver driver) : base(driver)
        {
        }

        public string GetArticleTitle()
        {
            var titleElement = driver.FindElementWithWait(_articleTitleBy);
            Logger.Info($"Located article title element: {titleElement}");
            return titleElement.Text.Trim();
        }
    }
}