namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using ManagedWinapi;

    public partial class ScreenCapture : Form
    {
        public ScreenCapture()
        {
            InitializeComponent();

            SetupHotkeys();
            SetupGlobalHotkey();

            TopMost = true;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

            var virtualScreen = SystemInformation.VirtualScreen;
            Location = virtualScreen.Location;
            Size = virtualScreen.Size;

            _canvas.Size = Size;
            SetupCaptureCanvas(_canvas);
            ScreenToCanvas(_canvas);
        }

        private void SetupCaptureCanvas(PictureBox canvas)
        {
            _canvas.BackgroundImage = new Bitmap(canvas.Size.Width, canvas.Size.Height);
            canvas.Image = new Bitmap(canvas.Size.Width, canvas.Size.Height);

            new SelectionDrawer(new PictureBoxCanvasDecorator(canvas));
        }

        private void ScreenToCanvas(PictureBox canvas)
        {
            using (var graphics = Graphics.FromImage(canvas.BackgroundImage))
            {
                graphics.CopyFromScreen(Location.X, Location.Y, 0, 0, canvas.Size);
            }
        }
        
        private void SetupHotkeys()
        {
            KeyDown += HideOnEscape;
        }

        private void HideOnEscape(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Hide();
            }
        }

        private void SetupGlobalHotkey()
        {
            var hotkey = new Hotkey
            {
                KeyCode = Keys.PrintScreen,
                Enabled = true
            };

            hotkey.HotkeyPressed += StartNewScreenCapture;
        }

        private void StartNewScreenCapture(object sender, EventArgs e)
        {
            ScreenToCanvas(_canvas);
            Show();
            BringToFront();
            Activate();
        }
    }
}
