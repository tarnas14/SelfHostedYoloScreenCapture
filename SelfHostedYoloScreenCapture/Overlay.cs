namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Overlay : Form, SelectionCanvas
    {
        public Overlay(Rectangle virtualScreen, Color transparencyKey)
        {
            InitializeComponent();
            Opacity = 0.4;
            BackColor = Color.Black;
            TransparencyKey = transparencyKey;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(virtualScreen.X, virtualScreen.Y);
            Size = new Size(virtualScreen.Width, virtualScreen.Height);

            _selectionCanvas.Image = new Bitmap(Size.Width, Size.Height);
        }

        public Image Canvas
        {
            get { return _selectionCanvas.Image; }
        }

        public void InvalidateCanvas(Rectangle rectangle)
        {
            _selectionCanvas.Invalidate(rectangle);
        }
    }
}
