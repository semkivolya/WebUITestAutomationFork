using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace WebUITestAutomation.Tests
{
    public class JobListingPage : BasePage
    {
        private readonly By _jobDetailsBy = By.CssSelector("article.vacancy-details-23");

        public JobListingPage(IWebDriver driver, IConfiguration configuration) : base(driver, configuration)
        {
        }

        public string GetJobDetails()
        {
            return FindElement(_jobDetailsBy).Text;
        }
    }
}
