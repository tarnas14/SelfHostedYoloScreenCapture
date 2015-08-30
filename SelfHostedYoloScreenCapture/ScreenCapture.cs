namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Configuration;
    using Drawing;
    using PhotoUploading;
    using SelectingRectangle;

    public partial class ScreenCapture : Form
    {
        private SelectionDrawer _selectionDrawer;
        private readonly PhotoUploader _photoUploader;
        private readonly CaptureRectangleFactory _captureRectangleFactory;
        private DrawingMagic _drawingMagic;
        private OnOffMouseEvents _onOffSelectionMouseEvents;
        private OnOffMouseEvents _onOffDrawingMouseEvents;

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

            SetupCaptureCanvas();
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

        private void SetupCaptureCanvas()
        {
            var updatedRectangle = _captureRectangleFactory.GetRectangle();

            if (updatedRectangle.Location != Location || updatedRectangle.Size != Size)
            {
                InitialiseEverythingFromScratch(updatedRectangle);
            }
            else
            {
                _backgroundPictureBox.Image = new Bitmap(_backgroundPictureBox.Size.Width, _backgroundPictureBox.Size.Height);
                _canvas.BackgroundImage = new Bitmap(_canvas.Size.Width, _canvas.Size.Height);
            }

            using (var graphics = Graphics.FromImage(_backgroundPictureBox.BackgroundImage))
            {
                graphics.CopyFromScreen(Location.X, Location.Y, 0, 0, _backgroundPictureBox.Size);
            }
        }

        private void InitialiseEverythingFromScratch(Rectangle updatedRectangle)
        {
            Location = updatedRectangle.Location;
            Size = updatedRectangle.Size;

            _backgroundPictureBox.Size = Size;
            _backgroundPictureBox.BackgroundImage = new Bitmap(_backgroundPictureBox.Size.Width, _backgroundPictureBox.Size.Height);
            _backgroundPictureBox.Image = new Bitmap(_backgroundPictureBox.Size.Width, _backgroundPictureBox.Size.Height);

            _canvas.Size = Size;
            _canvas.BackgroundImage = new Bitmap(_canvas.Size.Width, _canvas.Size.Height);
            _canvas.Image = new Bitmap(_canvas.Size.Width, _canvas.Size.Height);
            _canvas.Parent = _backgroundPictureBox;
            _canvas.BackColor = Color.Transparent;

            var pictureBoxMouseEvents = new ControlMouseEvents(_canvas);
            _onOffSelectionMouseEvents = new OnOffMouseEvents(pictureBoxMouseEvents)
            {
                On = true
            };
            _selectionDrawer = new SelectionDrawer(new PictureBoxImageCanvas(_canvas), _onOffSelectionMouseEvents);
            _onOffDrawingMouseEvents = new OnOffMouseEvents(pictureBoxMouseEvents)
            {
                On = false
            };
            _drawingMagic = new DrawingMagic(new PictureBoxImageCanvas(_backgroundPictureBox),
                new PictureBoxBackgroundCanvas(_canvas), _onOffDrawingMouseEvents);
            _drawingMagic.OperationFinished += (sender, args) =>
            {
                _onOffSelectionMouseEvents.On = true;
                _onOffDrawingMouseEvents.On = false;
            };
        }

        private void SetupActionBox()
        {
            _selectionDrawer.RectangleSelected += _actionBox.DrawCloseTo;
            _selectionDrawer.NewSelectionStarted += _actionBox.HideActions;
            _actionBox.Upload += UploadSelection;
            _actionBox.ToolSelected += (sender, toolSelectedEventArgs) =>
            {
                _onOffSelectionMouseEvents.On = false; 
                _onOffDrawingMouseEvents.On = true;
            };
            _actionBox.ToolSelected += _drawingMagic.UseTool;
        }

        private void UploadSelection(object sender, EventArgs e)
        {
            Hide();
            _photoUploader.Upload(CaptureSelection(_selectionDrawer.Selection));
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

            Clipboard.SetImage(CaptureSelection(_selectionDrawer.Selection));
            Hide();
        }

        private Image CaptureSelection(Rectangle selection)
        {
            var pictureToClipboard = new Bitmap(selection.Width, selection.Height);
            var pictureRectangle = new RectangleF(new PointF(0, 0), new SizeF(pictureToClipboard.Width, pictureToClipboard.Height));
            using (var graphics = Graphics.FromImage(pictureToClipboard))
            {
                graphics.DrawImage(_backgroundPictureBox.BackgroundImage, pictureRectangle, selection, GraphicsUnit.Pixel);
                graphics.DrawImage(_backgroundPictureBox.Image, pictureRectangle, selection, GraphicsUnit.Pixel);
            }

            return pictureToClipboard;
        }

        public void StartNewScreenCapture(object sender, EventArgs e)
        {
            SetupCaptureCanvas();
            _selectionDrawer.ResetSelection();
            _actionBox.Hide();
            Show();
            BringToFront();
            Activate();
        }
    }
}
