using Microsoft.Extensions.Configuration;
using WebUITestAutomation.Business.PageObjects;

namespace WebUITestAutomation.Tests
{
    internal class HomeContext
    {
        public static HomePage Open(IConfiguration configuration)
        {
            var homePage = new HomePage(DriverHolder.Driver);
            homePage.Open(configuration["application"]);
            return homePage;
        }
    }
}