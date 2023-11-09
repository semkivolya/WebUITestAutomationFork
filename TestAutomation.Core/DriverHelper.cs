using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace TestAutomation.Core
{
    public static class DriverHelper
    {
        public static IWebElement? FindElementWithWait(this IWebDriver driver, By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            IWebElement? element = wait.Until<IWebElement>(d =>
            {
                IWebElement tempElement = d.FindElement(by);
                return tempElement.Displayed && tempElement.Enabled ? tempElement : null;
            });
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsWithWait(this IWebDriver driver, By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            var elements = wait.Until<ReadOnlyCollection<IWebElement>>(d =>
            {
                var tempElements = d.FindElements(by);
                return tempElements.Any() ? tempElements : null;
            });
            return elements;
        }
    }
}
