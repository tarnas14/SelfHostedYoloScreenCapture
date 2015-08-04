namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Configuration;
    using PhotoUploading;
    using SelectingRectangle;

    public partial class ScreenCapture : Form
    {
        private SelectionDrawer _selectionDrawer;
        private readonly PhotoUploader _photoUploader;
        private readonly CaptureRectangleFactory _captureRectangleFactory;

        public ScreenCapture(TrayIcon trayIcon, PhotoUploader photoUploader, CaptureRectangleFactory captureRectangleFactory)
        {
            _photoUploader = photoUploader;
            _captureRectangleFactory = captureRectangleFactory;
            InitializeComponent();

            SetupIconEvents(trayIcon);

            SetupHotkeys();

            TopMost = true;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

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
            var updatedRectangle = _captureRectangleFactory.GetRectangle();

            if (updatedRectangle.Location == Location && updatedRectangle.Size == Size)
            {
                return;
            }

            Location = updatedRectangle.Location;
            Size = updatedRectangle.Size;

            canvas.Size = Size;
            canvas.BackgroundImage = new Bitmap(canvas.Size.Width, canvas.Size.Height);
            canvas.Image = new Bitmap(canvas.Size.Width, canvas.Size.Height);

            _selectionDrawer = new SelectionDrawer(new CanvasFromPictureBox(canvas));
        }

        private void SetupActionBox()
        {
            _selectionDrawer. RectangleSelected += _actionBox.DrawCloseTo;
            _selectionDrawer.NewSelectionStarted += _actionBox.HideActions;
            _actionBox.Upload += UploadSelection;
        }

        private void UploadSelection(object sender, EventArgs e)
        {
            Hide();
            _photoUploader.Upload(CaptureSelection(_canvas, _selectionDrawer.Selection));
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

        public void StartNewScreenCapture(object sender, EventArgs e)
        {
            SetupCaptureCanvas(_canvas);
            ScreenToCanvas(_canvas);
            _selectionDrawer.ResetSelection();
            _actionBox.Hide();
            Show();
            BringToFront();
            Activate();
        }
    }
}
