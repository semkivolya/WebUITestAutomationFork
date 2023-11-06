using OpenQA.Selenium;
using WebUITestAutomation.Core;

namespace WebUITestAutomation.Business.PageObjects
{
    public class HomePage : BasePage
    {
        private readonly By _acceptCookiesBy = By.Id("onetrust-accept-btn-handler");
        private readonly By _careersLinkBy = By.LinkText("Careers");
        private readonly By _aboutLinkBy = By.LinkText("About");
        private readonly By _insightsLinkBy = By.LinkText("Insights");

        private readonly By _globalSearchIconBy = By.XPath("//button[contains(@class,'header-search__button header__icon')]");
        private readonly By _globalSearchFieldBy = By.Id("new_form_search");
        private readonly By _globalSearchButtonBy = By.XPath("//*[@id='new_form_search']/../following-sibling::button");

        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public void Open(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public void AcceptCookies()
        {
            Click(() => driver.FindElementWithWait(_acceptCookiesBy));
        }

        public CareersPage ClickCareersLink()
        {
            Click(() => driver.FindElementWithWait(_careersLinkBy));
            return new CareersPage(driver);
        }

        public void ClickSearchIcon()
        {
            Click(() => driver.FindElementWithWait(_globalSearchIconBy));
        }

        public void EnterSearchString(string searchString)
        {
            SendKeys(() => driver.FindElementWithWait(_globalSearchFieldBy), searchString);
        }

        public SearchResultsPage ClickFindButton()
        {
            Click(() => driver.FindElement(_globalSearchButtonBy));
            return new SearchResultsPage(driver);
        }

        public AboutPage ClickAboutLink()
        {
            Click(() => driver.FindElementWithWait(_aboutLinkBy));
            return new AboutPage(driver);
        }

        public InsightsPage ClickInsightsLink()
        {
            Click(() => driver.FindElementWithWait(_insightsLinkBy));
            return new InsightsPage(driver);
        }
    }
}
