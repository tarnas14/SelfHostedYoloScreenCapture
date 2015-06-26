namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;
    using System.Windows.Forms;

    public partial class UserInput : Form, MouseEvents
    {
        public UserInput(Rectangle virtualScreen)
        {
            InitializeComponent();
            Opacity = 0.01;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(virtualScreen.X, virtualScreen.Y);
            Size = new Size(virtualScreen.Width, virtualScreen.Height);

            TopMost = true;
        }
    }
}
