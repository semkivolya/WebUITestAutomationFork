using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebUITestAutomation.Tests
{
    public class Tests
    {
        private IWebDriver driver;
        private IConfiguration configuration;

        [OneTimeSetUp]
        public void OnTimeSetUp()
        {
            configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        }

        [SetUp]
        public void SetUp()
        {
            if (bool.Parse(configuration["headless"]))
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
                driver = new ChromeDriver(options);
            }
            else
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
            }
        }

        [TestCase("c#", "All Locations")]
        [TestCase("java", "All Locations")]
        public void UserCanSearchForPositionBasedOnCriteria(string language, string location)
        {
            using (var homePage = new HomePage(driver, configuration))
            {
                homePage.Navigate();
                homePage.AcceptCookies();

                using (var careersPage = homePage.ClickCareersLink())
                {
                    careersPage.ScrollToJobSearchForm();
                    careersPage.EnterKeyword(language);
                    careersPage.EnterLocation(location);
                    careersPage.CheckRemoteOption();
                    careersPage.SubmitSearch();
                    careersPage.SortSearchResultsByDate();

                    using (var jobListingPage = careersPage.ViewLatestPostedJobDetails())
                    {
                        var jobDetails = jobListingPage.GetJobDetails();

                        Assert.That(jobDetails.Contains(language, StringComparison.OrdinalIgnoreCase));
                    }
                }
            }
        }

        [TestCase("\"BLOCKCHAIN\"/\"Cloud\"/\"Automation\"")]
        public void GlobalSearchWorksAsExpected(string searchString)
        {
            using (var homePage = new HomePage(driver, configuration))
            {
                homePage.Navigate();
                homePage.AcceptCookies();
                homePage.ClickSearchIcon();
                homePage.EnterSearchString(searchString);
                using (var searhResultsPage = homePage.ClickFindButton())
                {
                    var searchResultsText = searhResultsPage.GetSearchResultsText();

                    var searchTerms = searchString.Split("/").Select(t => t.Trim('"'));
                    var allSearchResultsContainSearchTerms = searchResultsText
                        .All(text =>
                        {
                            foreach (var term in searchTerms)
                            {
                                if (text.Contains(term, StringComparison.OrdinalIgnoreCase))
                                {
                                    return true;
                                }
                            }
                            return false;
                        });

                    Assert.That(allSearchResultsContainSearchTerms);
                }
            }
        }

        [TestCase("EPAM_Corporate_Overview_Q3_october.pdf")]
        [TestCase("EPAM_Systems_Company_Overview.pdf")]
        public async Task FileDownloadWorksAsExpetedAsync(string fileName)
        {
            using (var homePage = new HomePage(driver, configuration))
            {
                homePage.Navigate();
                homePage.AcceptCookies();
                using (var aboutPage = homePage.ClickAboutLink())
                {
                    aboutPage.ScrollToEpamAtGlanceSection();
                    var downloadedFilePath = await aboutPage.DownloadFileAsync();
                    var downloadedFileInfo = new FileInfo(downloadedFilePath);
                    Assert.That(downloadedFileInfo.Exists);
                    Assert.That(downloadedFileInfo.Length, Is.GreaterThan(0));
                    Assert.That(downloadedFileInfo.Name, Is.EqualTo(fileName));
                }
            }
        }

        [Test]
        public void TitleOfTheArticleMatchesWithTitleInCarousel()
        {
            using (var homePage = new HomePage(driver, configuration))
            {
                homePage.Navigate();
                homePage.AcceptCookies();
                using (var insightsPage = homePage.ClickInsightsLink())
                {
                    insightsPage.SwipeCarouselRight();
                    insightsPage.SwipeCarouselRight();
                    var carouselArticleTitle = insightsPage.GetCarouselArticleTitle();
                    using (var articlePage = insightsPage.ClickReadMore())
                    {
                        var pageArticleTitle = articlePage.GetArticleTitle();
                        Assert.That(pageArticleTitle, Is.EqualTo(carouselArticleTitle).IgnoreCase);
                    }
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }
    }
}

