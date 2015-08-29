namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Windows.Forms;
    using Configuration;
    using ManagedWinapi;
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

            var screenCapture = new ScreenCapture(_trayIcon, new PhotoUploaderPresentingResult(configuration.UploadPath, configuration.PictureGetPath), configuration);

            if (SetupGlobalHotkey(configuration.GlobalHotkey, screenCapture))
            {
                Application.Run(screenCapture);
            }

            _trayIcon.Dispose();
        }

        private bool SetupGlobalHotkey(HotkeyConfiguration globalHotkey, ScreenCapture screenCapture)
        {
            try
            {
                var hotkey = new Hotkey
                {
                    Alt = globalHotkey.Alt,
                    Ctrl = globalHotkey.Ctrl,
                    Shift = globalHotkey.Shift,
                    WindowsKey = globalHotkey.WindowsKey,
                    KeyCode = globalHotkey.KeyCode,
                    Enabled = true
                };

                hotkey.HotkeyPressed += screenCapture.StartNewScreenCapture;
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("This hotkey is already bound to an action. Change configuration to bind SelfHostedYoloScreenCapture to an available hotkey.","Hotkey taken",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
    }
}
