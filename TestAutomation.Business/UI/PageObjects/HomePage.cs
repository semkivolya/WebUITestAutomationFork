using OpenQA.Selenium;
using TestAutomation.Core;

namespace TestAutomation.Business.UI.PageObjects
{
    public class HomePage : BasePage
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            Logger.Info("Loaded home page");
        }

        public void AcceptCookies()
        {
            Click(() => driver.FindElementWithWait(_acceptCookiesBy));
            Logger.Info("Accepted cookies");
        }

        public CareersPage ClickCareersLink()
        {
            Click(() => driver.FindElementWithWait(_careersLinkBy));
            Logger.Info("Clocked 'careers' link");
            return new CareersPage(driver);
        }

        public void ClickSearchIcon()
        {
            Click(() => driver.FindElementWithWait(_globalSearchIconBy));
            Logger.Info("Clicked search icon");
        }

        public void EnterSearchString(string searchString)
        {
            SendKeys(() => driver.FindElementWithWait(_globalSearchFieldBy), searchString);
            Logger.Info($"Entered {searchString} into search field");
        }

        public SearchResultsPage ClickFindButton()
        {
            Click(() => driver.FindElement(_globalSearchButtonBy));
            Logger.Info("Submitted search");
            return new SearchResultsPage(driver);
        }

        public AboutPage ClickAboutLink()
        {
            Click(() => driver.FindElementWithWait(_aboutLinkBy));
            Logger.Info("Clicked 'about' link");
            return new AboutPage(driver);
        }

        public InsightsPage ClickInsightsLink()
        {
            Click(() => driver.FindElementWithWait(_insightsLinkBy));
            Logger.Info("Clicked 'insights' link");
            return new InsightsPage(driver);
        }
    }
}
