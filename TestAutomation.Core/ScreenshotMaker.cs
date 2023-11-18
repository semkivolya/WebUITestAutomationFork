using OpenQA.Selenium;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TestAutomation.Core
{
    public class ScreenshotMaker
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static class Win32Native
        {
            const int DESKTOPVERTRES = 0x75;
            const int DESKTOPHORZRES = 0x76;

            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hDC, int index);

            // Struct to store screen information
            [StructLayout(LayoutKind.Sequential)]
            public struct ScreenInfo
            {
                public string DeviceName;
                public Rectangle ScaledBounds;
            }

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr devMode);

            // Get the dimensions of all screens with scaling
            public static ScreenInfo[] GetScreensWithScaling()
            {
                ScreenInfo[] screens = new ScreenInfo[Screen.AllScreens.Length];
                int index = 0;

                foreach (var screen in Screen.AllScreens)
                {
                    IntPtr hdc = CreateDC("DISPLAY", screen.DeviceName, null, IntPtr.Zero);

                    var width = GetDeviceCaps(hdc, DESKTOPHORZRES);
                    var height = GetDeviceCaps(hdc, DESKTOPVERTRES);

                    Rectangle bounds = screen.Bounds;

                    var scaleX = (float)bounds.Width / width;
                    var scaleY = (float)bounds.Height / height;

                    Rectangle scaledBounds = new Rectangle(
                                            (int)(bounds.X / scaleX),
                                            (int)(bounds.Y / scaleY),
                                            width,
                                            height
                                        );

                    screens[index] = new ScreenInfo
                    {
                        DeviceName = screen.DeviceName,
                        ScaledBounds = scaledBounds,
                    };

                    index++;
                }

                return screens;
            }
        }
        private static string ScreenshotsFolder => "Screenshots";
        private static string NewScreenshotName => $"_{DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff")}.{ScreenshotImageFormat}";
        private static ImageFormat ScreenshotImageFormat => ImageFormat.Png;

        private static string DisplayScreenshotName => "Display" + NewScreenshotName;

        private static string FullScreenScreenshotName => "FullScreen" + NewScreenshotName;

        public static string TakeBrowserScreenshot(IWebDriver driver)
        {
            if (!Directory.Exists(ScreenshotsFolder))
            {
                Logger.Info($"Created screenshots directory: {Path.GetFullPath(ScreenshotsFolder)}");
                Directory.CreateDirectory(ScreenshotsFolder);
            }
            var screenshotPath = Path.Combine(ScreenshotsFolder, DisplayScreenshotName);
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            Logger.Info($"Took screenshot: {Path.GetFullPath(screenshotPath)}");
            screenshot.SaveAsFile(screenshotPath);
            return screenshotPath;
        }

        public static string TakeFullDisplayScreenshot()
        {
            if (!Directory.Exists(ScreenshotsFolder))
            {
                Logger.Info($"Created screenshots directory: {Path.GetFullPath(ScreenshotsFolder)}");
                Directory.CreateDirectory(ScreenshotsFolder);
            }
            var screenshotPath = Path.Combine(ScreenshotsFolder, FullScreenScreenshotName);

            int totalWidth = 0;
            int maxHeight = 0;
            var allScreens = Win32Native.GetScreensWithScaling();

            for (int i = 0; i < allScreens.Length; i++)
            {
                Win32Native.ScreenInfo screen = allScreens[i];
                totalWidth += screen.ScaledBounds.Width;
                if (screen.ScaledBounds.Height > maxHeight)
                {
                    maxHeight = screen.ScaledBounds.Height;
                }
            }

            using (Bitmap bitmap = new Bitmap(totalWidth, maxHeight))
            {
                int xOffset = 0;
                foreach (var screen in allScreens)
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(
                            new Point(screen.ScaledBounds.X, screen.ScaledBounds.Y),
                            new Point(xOffset, 0),
                            new Size(screen.ScaledBounds.Width, screen.ScaledBounds.Height)
                        );
                    }

                    xOffset += screen.ScaledBounds.Width;
                }
                Logger.Info($"Took screenshot: {Path.GetFullPath(screenshotPath)}");
                bitmap.Save(screenshotPath, ImageFormat.Png);
            }

            return screenshotPath;
        }
    }
}
