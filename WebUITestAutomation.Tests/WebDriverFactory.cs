using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace WebUITestAutomation.Tests
{
    public class WebDriverFactory
    {
        public static IWebDriver GetDriver(IConfiguration configuration)
        {
            var browserName = configuration["browser"]?.ToUpperInvariant();
            bool headless = bool.Parse(configuration["headless"]);
            switch (browserName.ToUpperInvariant())
            {
                case "CHROME":
                    var chromeOptions = new ChromeOptions();
                    if (headless)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }
                    return new ChromeDriver(chromeOptions);
                case "FIREFOX":
                    var firefoxOptions = new FirefoxOptions();
                    if (headless)
                    {
                        firefoxOptions.AddArgument("--headless=new");
                        firefoxOptions.AddArgument("--window-size=1920,1080");
                    }
                    return new FirefoxDriver(firefoxOptions);
                case "EDGE":
                    var edgeOptions = new EdgeOptions();
                    if (headless)
                    {
                        edgeOptions.AddArgument("--headless=new");
                        edgeOptions.AddArgument("--window-size=1920,1080");
                    }
                    return new EdgeDriver(edgeOptions);
                default:
                    throw new ArgumentException("Not supported browser", nameof(browserName));
            }
        }
    }
}
