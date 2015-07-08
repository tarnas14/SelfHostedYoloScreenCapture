namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Configuration;
    using PhotoUploading;

    class Program
    {
        private TrayIcon _trayIcon;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            _trayIcon = new TrayIcon();

            var configuration = ConfigurationFactory.FromFile<ScreenCaptureConfiguration>("screenCaptureConfig.json");

            Application.Run(new ScreenCapture(_trayIcon, new PhotoUploaderPresentingResult(configuration.ServerPath), ConfigureCaptureRectangle(configuration)));

            _trayIcon.Dispose();
        }

        private Rectangle ConfigureCaptureRectangle(ScreenCaptureConfiguration screenCaptureConfiguration)
        {
            if (screenCaptureConfiguration.Fullscreen)
            {
                return SystemInformation.VirtualScreen;
            }

            return screenCaptureConfiguration.CustomCaptureRectangle;
        }
    }
}
