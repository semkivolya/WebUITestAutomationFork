using OpenQA.Selenium;

namespace WebUITestAutomation.Tests.PageObjects
{
    public class JobListingPage : BasePage
    {
        private readonly By _jobDetailsBy = By.CssSelector("article.vacancy-details-23");

        public JobListingPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetJobDetails()
        {
            return FindElement(_jobDetailsBy).Text;
        }
    }
}
