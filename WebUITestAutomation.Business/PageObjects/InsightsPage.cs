using OpenQA.Selenium;
using WebUITestAutomation.Core;

namespace WebUITestAutomation.Business.PageObjects
{
    public class InsightsPage : BasePage
    {
        private readonly By _articlesCarouselRightArrowBy = By.XPath("//*[contains(@class, 'slider section')][1]//*[contains(@class, 'slider__right-arrow')]");
        private readonly By _activeArticleLinkBy = By.XPath("//*[contains(@class, 'slider section')][1]//div[contains(@class,'owl-item active')]//a");
        private readonly By _activeArticleTitleElementsBy = By.XPath("//*[contains(@class, 'slider section')][1]//div[contains(@class,'owl-item active')]//p/span/span");
        public InsightsPage(IWebDriver driver) : base(driver)
        {
        }

        public void SwipeCarouselRight()
        {
            Click(() => driver.FindElementWithWait(_articlesCarouselRightArrowBy));
            Thread.Sleep(500);
        }

        public ArticlePage ClickReadMore()
        {
            Click(() => driver.FindElementWithWait(_activeArticleLinkBy));
            return new ArticlePage(driver);
        }

        public string GetCarouselArticleTitle()
        {
            var articleTitleElements = driver.FindElementsWithWait(_activeArticleTitleElementsBy);
            return string.Join(' ', articleTitleElements.Select(t => t.Text.Trim()));
        }
    }
}