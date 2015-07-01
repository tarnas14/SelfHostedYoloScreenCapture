namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using ManagedWinapi;

    public partial class ScreenCapture : Form
    {
        private SelectionDrawer _selectionDrawer;

        public ScreenCapture(TrayIcon trayIcon)
        {
            InitializeComponent();

            SetupIconEvents(trayIcon);

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

        private void SetupIconEvents(TrayIcon trayIcon)
        {
            trayIcon.Exit += Exit;
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void SetupCaptureCanvas(PictureBox canvas)
        {
            _canvas.BackgroundImage = new Bitmap(canvas.Size.Width, canvas.Size.Height);
            canvas.Image = new Bitmap(canvas.Size.Width, canvas.Size.Height);

            _selectionDrawer = new SelectionDrawer(new PictureBoxCanvasDecorator(canvas));
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
            KeyDown += CopyToClipboard;
        }

        private void HideOnEscape(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Hide();
            }
        }

        private void CopyToClipboard(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                Clipboard.SetImage(CaptureSelection(_canvas, _selectionDrawer.Selection));
                Hide();
            }
        }

        private Image CaptureSelection(PictureBox canvas, Rectangle selection)
        {
            var pictureToClipboard = new Bitmap(selection.Width, selection.Height);
            var pictureRectangle = new RectangleF(new PointF(0, 0),
                new SizeF(pictureToClipboard.Width, pictureToClipboard.Height));
            using (var graphics = Graphics.FromImage(pictureToClipboard))
            {
                graphics.DrawImage(canvas.BackgroundImage, pictureRectangle, selection, GraphicsUnit.Pixel);
            }

            return pictureToClipboard;
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
