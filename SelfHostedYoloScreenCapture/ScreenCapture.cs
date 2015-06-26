namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;
    using System.Windows.Forms;

    public partial class ScreenCapture : Form
    {
        public ScreenCapture()
        {
            InitializeComponent();

            TopMost = true;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

            var virtualScreen = SystemInformation.VirtualScreen;
            Location = virtualScreen.Location;
            Size = virtualScreen.Size;

            _canvas.Size = virtualScreen.Size;
            _canvas.BackgroundImage = new Bitmap(virtualScreen.Size.Width, virtualScreen.Size.Height);
            using (var graphics = Graphics.FromImage(_canvas.BackgroundImage))
            {
                graphics.CopyFromScreen(virtualScreen.Left, virtualScreen.Top, 0, 0, virtualScreen.Size);
            }

            _canvas.Image = new Bitmap(virtualScreen.Size.Width, virtualScreen.Size.Height);
            new SelectionDrawer(new PictureBoxCanvasDecorator(_canvas));
        }
    }
}
