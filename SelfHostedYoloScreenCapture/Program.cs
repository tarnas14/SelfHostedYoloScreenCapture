namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
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

            //var captureRectangle = new Rectangle(new Point(0, 0), new Size(800, 600));
            var captureRectangle = SystemInformation.VirtualScreen;
            Application.Run(new ScreenCapture(_trayIcon, new PhotoUploader(), captureRectangle));

            _trayIcon.Dispose();
        }
    }
}
