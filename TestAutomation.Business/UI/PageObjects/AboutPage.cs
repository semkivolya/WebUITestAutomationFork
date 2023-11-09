using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Runtime.InteropServices;
using TestAutomation.Core;

namespace TestAutomation.Business.UI.PageObjects
{
    public class AboutPage : BasePage
    {
        private bool _disposed;
        private readonly string _fileDownloadFolder;
        private string _downloadedFilePath;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly By _epamAtAGlanceBy = By.CssSelector(".content-container>div:nth-child(5)");
        private readonly By _downloadLinkBy = By.CssSelector(".content-container>div:nth-child(5) .button a");
        public AboutPage(IWebDriver driver) : base(driver)
        {
            _fileDownloadFolder = Guid.NewGuid().ToString();
        }

        public void ScrollToEpamAtGlanceSection()
        {
            var section = driver.FindElementWithWait(_epamAtAGlanceBy);
            new Actions(driver).ScrollToElement(section).Perform();
            Logger.Info("Scrolled to the Epam at a glance section");
        }

        public async Task<string> DownloadFileAsync()
        {
            var downloadLink = driver.FindElementWithWait(_downloadLinkBy);
            var address = downloadLink.GetAttribute("href");
            Logger.Info($"Link to the file: {address}");

            var fileName = Path.GetFileName(address);
            if (!Directory.Exists(_fileDownloadFolder))
            {
                Directory.CreateDirectory(_fileDownloadFolder);
            }
            _downloadedFilePath = Path.Combine(_fileDownloadFolder, fileName);
            if (File.Exists(_downloadedFilePath))
            {
                File.Delete(_downloadedFilePath);
            }
            var fileStream = await HttpClient.GetStreamAsync(address);
            using (var outputFileStream = new FileStream(_downloadedFilePath, FileMode.CreateNew))
            {
                fileStream.CopyTo(outputFileStream);
            }
            Logger.Info($"Saved file to the {_downloadedFilePath} location");
            return _downloadedFilePath;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Directory.Exists(_fileDownloadFolder))
                    {
                        Directory.Delete(_fileDownloadFolder, true);
                        Logger.Info("Removed downloaded file");
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}