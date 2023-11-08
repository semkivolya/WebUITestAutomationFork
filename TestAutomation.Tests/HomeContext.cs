using Microsoft.Extensions.Configuration;
using TestAutomation.Business.UI.PageObjects;

namespace TestAutomation.Tests
{
    internal class HomeContext
    {
        public static HomePage Open(IConfiguration configuration)
        {
            var homePage = new HomePage(DriverHolder.Driver);
            homePage.Open(configuration["uiapplication"]);
            return homePage;
        }
    }
}