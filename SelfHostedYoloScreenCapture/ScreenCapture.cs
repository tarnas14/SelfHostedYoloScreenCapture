namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using ManagedWinapi;

    public partial class ScreenCapture : Form
    {
        private SelectionDrawer _selectionDrawer;
        private readonly PhotoUploader _photoUploader;

        public ScreenCapture(TrayIcon trayIcon, PhotoUploader photoUploader, Rectangle captureRectangle)
        {
            _photoUploader = photoUploader;
            InitializeComponent();

            SetupIconEvents(trayIcon);

            SetupHotkeys();
            SetupGlobalHotkey();

            TopMost = true;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

            Location = captureRectangle.Location;
            Size = captureRectangle.Size;

            _canvas.Size = Size;
            SetupCaptureCanvas(_canvas);
            ScreenToCanvas(_canvas);
            SetupActionBox();
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

        private void SetupActionBox()
        {
            _selectionDrawer.RectangleSelected += _actionBox.DrawCloseTo;
            _selectionDrawer.NewSelectionStarted += _actionBox.HideActions;
            _actionBox.Upload += (sender, args) => _photoUploader.Upload(CaptureSelection(_canvas, _selectionDrawer.Selection));
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
            _actionBox.KeyDownBubble += HideOnEscape;
            KeyDown += CopyToClipboard;
            _actionBox.KeyDownBubble += CopyToClipboard;
        }

        private void HideOnEscape(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape || e.Control || e.Alt || e.Shift)
            {
                return;
            }

            Hide();
        }

        private void CopyToClipboard(object sender, KeyEventArgs args)
        {
            if (!args.Control || args.KeyCode != Keys.C || args.Shift || args.Alt)
            {
                return;
            }

            Clipboard.SetImage(CaptureSelection(_canvas, _selectionDrawer.Selection));
            Hide();
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
            _selectionDrawer.ResetSelection();
            _actionBox.Hide();
            Show();
            BringToFront();
            Activate();
        }
    }
}
