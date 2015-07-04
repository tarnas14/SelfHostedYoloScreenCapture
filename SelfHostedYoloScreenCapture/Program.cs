namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Configuration;

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

            Application.Run(new ScreenCapture(_trayIcon, new PhotoUploader(), ConfigureCaptureRectangle()));

            _trayIcon.Dispose();
        }

        private Rectangle ConfigureCaptureRectangle()
        {
            var configuration = ConfigurationFactory.FromFile<ScreenCaptureConfiguration>("screenCaptureConfig.json");

            if (configuration.Fullscreen)
            {
                return SystemInformation.VirtualScreen;
            }

            return configuration.CustomCaptureRectangle;
        }
    }
}
