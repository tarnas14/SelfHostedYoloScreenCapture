using System.Windows.Forms;

namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;

    public partial class Form1 : Form
    {
        private bool _choosingRectangle;
        private Point _startLocation;
        private Point _endLocation;


        public Form1(Rectangle virtualScreen)
        {
            InitializeComponent();
            BackColor = Color.Black;
            Opacity = 0.4;
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(virtualScreen.X, virtualScreen.Y);
            Size = new Size(virtualScreen.Width, virtualScreen.Height);

            TransparencyKey = Color.LimeGreen;

            pictureBox1.Size = Size; 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);

            using (var brush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                g.FillRectangle(brush, new Rectangle(new Point(0,0), Size));
            }

            pictureBox1.MouseDown += OnMouseDown;
            pictureBox1.MouseMove += OnMouseMove;
            pictureBox1.MouseUp += OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _choosingRectangle = true;
            _startLocation = e.Location;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_choosingRectangle)
            {
                _endLocation = e.Location;
                DrawRectangle();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _choosingRectangle = false;
            _endLocation = e.Location;

            DrawRectangle();
        }

        private void DrawRectangle()
        {
            using (var brush = new SolidBrush(Color.LimeGreen))
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                var size = new Size(_endLocation.X - _startLocation.X, _endLocation.Y - _startLocation.Y);
                var transparentRectangle = new Rectangle(_startLocation, size);
                g.FillRectangle(brush, transparentRectangle);
                pictureBox1.Invalidate();
            }
        }
    }
}
