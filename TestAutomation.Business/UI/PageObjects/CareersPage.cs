using OpenQA.Selenium;
using TestAutomation.Core;

namespace TestAutomation.Business.UI.PageObjects
{
    public class CareersPage : BasePage
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly By _jobSearchBlockBy = By.CssSelector(".job-search.recruiting-search");
        private readonly By _keywordsFieldBy = By.Id("new_form_job_search-keyword");
        private readonly By _locationsListBy = By.XPath("//ul[@id='select2-new_form_job_search-location-results']//ul//li");
        private readonly By _locationsSelectionArrowBy = By.ClassName("select2-selection__arrow");
        private readonly By _autocompleteBy = By.ClassName("autocomplete-suggestions");
        private readonly By _remoteCheckBy = By.XPath("//p[contains(@class, 'job-search__filter-items--remote')]/input[@name='remote']/following-sibling::label");
        private readonly By _findButtonBy = By.XPath("//*[@id='jobSearchFilterForm']/button[@type='submit']");
        private readonly By _sortByDateBy = By.XPath("//input[@id='sort-time']//following-sibling::label");
        private readonly By _viewAndApplyBy = By.CssSelector(".search-result__item:first-of-type .search-result__item-controls a");

        public CareersPage(IWebDriver driver) : base(driver)
        {
        }

        public void ScrollToJobSearchForm()
        {
            IJavaScriptExecutor jsExecutor = driver as IJavaScriptExecutor;
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(_jobSearchBlockBy));
            Logger.Info("Scrolled to search form");
        }

        public void EnterKeyword(string language)
        {
            SendKeys(() => driver.FindElementWithWait(_keywordsFieldBy), language);
            Logger.Info($"Entered {language} into the keyword field");
        }

        public void EnterLocation(string location)
        {
            Click(() => driver.FindElementWithWait(_locationsSelectionArrowBy));
            Click(() =>
            {
                var locations = driver.FindElementsWithWait(_locationsListBy);
                return locations.FirstOrDefault(x => x.GetAttribute("title") == location);
            });
            Logger.Info($"Selected {location} location");
        }

        public void CheckRemoteOption()
        {
            try
            {
                Click(() => driver.FindElementWithWait(_remoteCheckBy));
                Logger.Info("Selected 'remote' option");
            }
            catch (ElementClickInterceptedException e)
            {
                Logger.Warn(e, $"Exception while selecting 'remote' option");
                var autocomplete = driver.FindElement(_autocompleteBy);
                var executor = driver as IJavaScriptExecutor;
                executor.ExecuteScript("arguments[0].style.display='none';", autocomplete);
                Logger.Info("Hide autocomplete window");
            }
        }

        public void SubmitSearch()
        {
            Click(() => driver.FindElementWithWait(_findButtonBy));
            Logger.Info("Submitted search form");
        }

        public void SortSearchResultsByDate()
        {
            Click(() => driver.FindElementWithWait(_sortByDateBy));
            Logger.Info("Sorted jobs by date");
        }

        public JobListingPage ViewLatestPostedJobDetails()
        {
            Click(() => driver.FindElementWithWait(_viewAndApplyBy));
            Logger.Info("Selected the latest posted job");
            return new JobListingPage(driver);
        }
    }
}
