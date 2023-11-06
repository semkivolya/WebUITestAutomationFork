using OpenQA.Selenium;

namespace WebUITestAutomation.Tests.PageObjects
{
    public class CareersPage : BasePage
    {

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

        }

        public void EnterKeyword(string language)
        {
            SendKeys(() => FindElement(_keywordsFieldBy), language);
        }

        public void EnterLocation(string location)
        {
            Click(() => FindElement(_locationsSelectionArrowBy));
            Click(() =>
            {
                var locations = FindElements(_locationsListBy);
                return locations.FirstOrDefault(x => x.GetAttribute("title") == location);
            });
        }

        public void CheckRemoteOption()
        {
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
        }

        public void SubmitSearch()
        {
            Click(() => FindElement(_findButtonBy));
        }

        public void SortSearchResultsByDate()
        {
            Click(() => FindElement(_sortByDateBy));
        }

        public JobListingPage ViewLatestPostedJobDetails()
        {
            Click(() => FindElement(_viewAndApplyBy));
            return new JobListingPage(driver);
        }
    }
}
