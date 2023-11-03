using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebUITestAutomation.Tests
{
    public class InsightsPage : BasePage
    {
        private readonly By _articlesCarouselRightArrowBy = By.XPath("//*[contains(@class, 'slider section')][1]//*[contains(@class, 'slider__right-arrow')]");
        private readonly By _activeArticleLinkBy = By.XPath("//*[contains(@class, 'slider section')][1]//div[contains(@class,'owl-item active')]//a");
        private readonly By _activeArticleTitleElementsBy = By.XPath("//*[contains(@class, 'slider section')][1]//div[contains(@class,'owl-item active')]//p/span/span");
        public InsightsPage(IWebDriver driver, IConfiguration configuration) : base(driver, configuration)
        {
        }

        public void SwipeCarouselRight()
        {
            Click(() => FindElement(_articlesCarouselRightArrowBy));
            Thread.Sleep(500);
        }

        public ArticlePage ClickReadMore()
        {
            Click(() => FindElement(_activeArticleLinkBy));
            return new ArticlePage(driver, configuration);
        }

        public string GetCarouselArticleTitle()
        {
            var articleTitleElements = FindElements(_activeArticleTitleElementsBy);
            return string.Join(' ', articleTitleElements.Select(t => t.Text.Trim()));
        }
    }
}