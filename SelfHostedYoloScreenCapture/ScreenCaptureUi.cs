namespace SelfHostedYoloScreenCapture
{
    using System.Windows.Forms;
    using System.Drawing;

    public partial class ScreenCaptureUi : Form
    {
        private bool _choosingRectangle;
        private Point _startLocation;
        private Point _endLocation;


        public ScreenCaptureUi(Rectangle virtualScreen)
        {
            InitializeComponent();
            BackColor = Color.Black;
            Opacity = 0.4;
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(virtualScreen.X, virtualScreen.Y);
            Size = new Size(virtualScreen.Width, virtualScreen.Height);

            TransparencyKey = Color.LimeGreen;

            _overlayBox.Size = Size; 
            _overlayBox.BackColor = Color.Transparent;
            _overlayBox.Image = new Bitmap(_overlayBox.Size.Width, _overlayBox.Size.Height);

            new SelectionDrawer(_overlayBox, TransparencyKey);
        }
    }
}
