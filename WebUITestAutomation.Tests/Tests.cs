using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace WebUITestAutomation.Tests
{
    public class Tests
    {
        private string? _webAppUrl;
        private IWebDriver driver;
        private IConfiguration configuration;
        private readonly By _careersLinkBy = By.LinkText("Careers");
        private readonly By _findButtonBy = By.XPath("//*[@id='jobSearchFilterForm']/button[@type='submit']");
        private readonly By _jobSearchBlockBy = By.CssSelector(".job-search.recruiting-search");
        private readonly By _keywordsFieldBy = By.Id("new_form_job_search-keyword");
        private readonly By _autocompleteBy = By.ClassName("autocomplete-suggestions");
        private readonly By _locationsListBy = By.XPath("//ul[@id='select2-new_form_job_search-location-results']//ul//li");

        private readonly By _locationsSelectionArrowBy = By.ClassName("select2-selection__arrow");
        private readonly By _remoteCheckBy = By.XPath("//p[contains(@class, 'job-search__filter-items--remote')]/input[@name='remote']/following-sibling::label");
        private readonly By _sortByDateBy = By.XPath("//input[@id='sort-time']//following-sibling::label");
        private readonly By _viewAndApplyBy = By.CssSelector(".search-result__item:first-of-type .search-result__item-controls a");
        private readonly By _vacancyDetailsBy = By.CssSelector("article.vacancy-details-23");
        private readonly By _acceptCookiesBy = By.Id("onetrust-accept-btn-handler");

        private readonly By _globalSearchIconBy = By.XPath("//button[contains(@class,'header-search__button header__icon')]");
        private readonly By _globalSearchFieldBy = By.Id("new_form_search");
        private readonly By _globalSearchButtonBy = By.XPath("//*[@id='new_form_search']/../following-sibling::button");
        private readonly By _searchResultItemTextBy = By.CssSelector(".search-results__item p");

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            _webAppUrl = configuration["webAppUrl"];
        }

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
        }

        [TestCase("c#", "All Locations")]
        [TestCase("java", "All Locations")]
        public void UserCanSearchForPositionBasedOnCriteria(string language, string location)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(_webAppUrl);

            Click(() => FindElement(_acceptCookiesBy));

            Click(() => FindElement(_careersLinkBy));

            IJavaScriptExecutor jsExecutor = driver as IJavaScriptExecutor;
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(_jobSearchBlockBy));

            SendKeys(() => FindElement(_keywordsFieldBy), language);
            Click(() => FindElement(_locationsSelectionArrowBy));
            Click(() =>
            {
                var locations = FindElements(_locationsListBy);
                return locations.FirstOrDefault(x => x.GetAttribute("title") == location);
            });
            try
            {
                Click(() => FindElement(_remoteCheckBy));
            }
            catch (ElementClickInterceptedException)
            {
                var autocomplete = driver.FindElement(_autocompleteBy);
                var executor = driver as IJavaScriptExecutor;
                executor.ExecuteScript("arguments[0].style.display='none';", autocomplete);
            }
            Click(() => FindElement(_findButtonBy));
            Click(() => FindElement(_sortByDateBy));
            Click(() => FindElement(_viewAndApplyBy));

            var vacancyDetails = FindElement(_vacancyDetailsBy).Text;

            Assert.That(vacancyDetails.Contains(language, StringComparison.OrdinalIgnoreCase));
        }

        [TestCase("\"BLOCKCHAIN\"/\"Cloud\"/\"Automation\"")]
        public void GlobalSearchWorksAsExpected(string searchString)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(_webAppUrl);

            Click(() => FindElement(_acceptCookiesBy));
            Click(() => FindElement(_globalSearchIconBy));
            SendKeys(() => FindElement(_globalSearchFieldBy), searchString);
            Click(() => driver.FindElement(_globalSearchButtonBy));

            var searchTerms = searchString.Split("/").Select(t => t.Trim('"'));
            var searchResults = FindElements(_searchResultItemTextBy).Select(p => p.Text).ToList();
            var allLinksContainSearchTerms = searchResults
                .All(text =>
                {
                    foreach (var term in searchTerms)
                    {
                        if (text.Contains(term, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                });

            Assert.That(allLinksContainSearchTerms);
        }

        private IWebElement? FindElement(By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            IWebElement? element = wait.Until<IWebElement>(d =>
            {
                IWebElement tempElement = d.FindElement(by);
                return tempElement.Displayed && tempElement.Enabled ? tempElement : null;
            });
            return element;
        }

        private ReadOnlyCollection<IWebElement> FindElements(By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            var elements = wait.Until<ReadOnlyCollection<IWebElement>>(d =>
            {
                var tempElements = d.FindElements(by);
                return tempElements.Any() ? tempElements : null;
            });
            return elements;
        }

        private void Click(Func<IWebElement> getElement)
        {
            try
            {
                var element = getElement();
                element.Click();
            }
            catch (Exception e) when (e is ElementClickInterceptedException || e is StaleElementReferenceException)
            {
                var element = getElement();
                element.Click();
            }
        }

        private void SendKeys(Func<IWebElement> getElement, string value)
        {
            try
            {
                var element = getElement();
                element.Clear();
                element.SendKeys(value);
            }
            catch (StaleElementReferenceException)
            {
                var element = getElement();
                element.Clear();
                element.SendKeys(value);
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }
    }
}

/*var configuration = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.Build();
var address = configuration["webAppUrl"];
return address;*/
