using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace WebUITestAutomation.Tests
{
    public class HomePage : BasePage
    {
        private string? _webAppUrl;

        private readonly By _acceptCookiesBy = By.Id("onetrust-accept-btn-handler");
        private readonly By _careersLinkBy = By.LinkText("Careers");
        private readonly By _aboutLinkBy = By.LinkText("About");
        private readonly By _insightsLinkBy = By.LinkText("Insights");

        private readonly By _globalSearchIconBy = By.XPath("//button[contains(@class,'header-search__button header__icon')]");
        private readonly By _globalSearchFieldBy = By.Id("new_form_search");
        private readonly By _globalSearchButtonBy = By.XPath("//*[@id='new_form_search']/../following-sibling::button");

        public HomePage(IWebDriver driver, IConfiguration configuration) : base(driver, configuration)
        {
            _webAppUrl = configuration["webAppUrl"];
        }

        public void Navigate()
        {
            driver.Navigate().GoToUrl(_webAppUrl);
        }

        public void AcceptCookies()
        {
            Click(() => FindElement(_acceptCookiesBy));
        }

        public CareersPage ClickCareersLink()
        {
            Click(() => FindElement(_careersLinkBy));
            return new CareersPage(driver, configuration);
        }

        public void ClickSearchIcon()
        {
            Click(() => FindElement(_globalSearchIconBy));
        }

        public void EnterSearchString(string searchString)
        {
            SendKeys(() => FindElement(_globalSearchFieldBy), searchString);
        }

        public SearchResultsPage ClickFindButton()
        {
            Click(() => driver.FindElement(_globalSearchButtonBy));
            return new SearchResultsPage(driver, configuration);
        }

        public AboutPage ClickAboutLink()
        {
            Click(() => FindElement(_aboutLinkBy));
            return new AboutPage(driver, configuration);
        }

        public InsightsPage ClickInsightsLink()
        {
            Click(() => FindElement(_insightsLinkBy));
            return new InsightsPage(driver, configuration);
        }
    }
}
