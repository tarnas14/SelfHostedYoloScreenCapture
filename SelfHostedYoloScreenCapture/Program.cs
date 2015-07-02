namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Windows.Forms;

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

            Application.Run(new ScreenCapture(_trayIcon, new PhotoUploader()));

            _trayIcon.Dispose();
        }
    }
}
