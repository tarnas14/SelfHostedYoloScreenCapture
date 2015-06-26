using System;
using System.Windows.Forms;

namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;
    using System.Threading;

    class Program
    {
        private readonly AutoResetEvent _overlayInitialized = new AutoResetEvent(false);
        private Thread _overlayThread;
        private Thread _userInputThread;
        private readonly AutoResetEvent _userInputInitialized = new AutoResetEvent(false);
        private Color _transparencyKey;
        private Overlay _overlayForm;
        private UserInput _userInputForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            _transparencyKey = Color.LimeGreen;

            var virtualScreen = SystemInformation.VirtualScreen;
            _overlayForm = new Overlay(virtualScreen, _transparencyKey);
            _overlayThread = new Thread(OverlayThread);
            _overlayThread.IsBackground = false;
            _overlayThread.Start();
            _overlayInitialized.WaitOne();

            _userInputForm = new UserInput(virtualScreen);
            _userInputThread = new Thread(UserInputThread);
            _userInputThread.IsBackground = false;
            _userInputThread.Start();
            _userInputInitialized.WaitOne();

            while (_overlayThread.ThreadState != ThreadState.Running ||
                   _userInputThread.ThreadState != ThreadState.Running)
            { }

            new SelectionDrawer(_overlayForm, _userInputForm, _transparencyKey);
        }

        [STAThread]
        void OverlayThread()
        {
            _overlayInitialized.Set();
            Application.Run(_overlayForm);
        }

        [STAThread]
        private void UserInputThread()
        {
            _userInputInitialized.Set();
            Application.Run(_userInputForm);
        }
    }
}
