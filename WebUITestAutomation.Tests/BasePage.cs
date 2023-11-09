using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace WebUITestAutomation.Tests
{
    public class BasePage : IDisposable
    {
        protected IWebDriver driver;
        protected IConfiguration configuration;
        private bool disposed;
        public static HttpClient HttpClient { get; } = new HttpClient();

        public BasePage(IWebDriver driver, IConfiguration configuration)
        {
            this.driver = driver;
        }

        protected IWebElement? FindElement(By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            IWebElement? element = wait.Until<IWebElement>(d =>
            {
                IWebElement tempElement = d.FindElement(by);
                return tempElement.Displayed && tempElement.Enabled ? tempElement : null;
            });
            return element;
        }

        protected ReadOnlyCollection<IWebElement> FindElements(By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            var elements = wait.Until<ReadOnlyCollection<IWebElement>>(d =>
            {
                var tempElements = d.FindElements(by);
                return tempElements.Any() ? tempElements : null;
            });
            return elements;
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