using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Runtime.InteropServices;

namespace WebUITestAutomation.Tests
{
    public class AboutPage : BasePage
    {
        private bool _disposed;
        private readonly string _fileDownloadFolder;
        private string _downloadedFilePath;
        private readonly By _epamAtAGlanceBy = By.CssSelector(".content-container>div:nth-child(5)");
        private readonly By _downloadLinkBy = By.CssSelector(".content-container>div:nth-child(5) .button a");
        public AboutPage(IWebDriver driver, IConfiguration configuration) : base(driver, configuration)
        {
            _fileDownloadFolder = Guid.NewGuid().ToString();
        }

        public void ScrollToEpamAtGlanceSection()
        {
            var section = FindElement(_epamAtAGlanceBy);
            new Actions(driver).ScrollToElement(section).Perform();
        }

        public async Task<string> DownloadFileAsync()
        {
            var downloadLink = FindElement(_downloadLinkBy);
            var address = downloadLink.GetAttribute("href");

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
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}