using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace WebUITestAutomation.Business.PageObjects
{
    public class BasePage : IDisposable
    {
        protected IWebDriver driver;
        private bool disposed;

        public static HttpClient HttpClient { get; } = new HttpClient();

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected void Click(Func<IWebElement> getElement)
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

        protected void SendKeys(Func<IWebElement> getElement, string value)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}