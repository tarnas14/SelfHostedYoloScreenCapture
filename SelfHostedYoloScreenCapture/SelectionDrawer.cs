namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SelectionDrawer
    {
        private readonly PictureBox _overlayBox;
        private readonly Color _transparencyKey;
        private bool _choosingRectangle;
        private Point _startLocation;

        public Rectangle Selection { get; private set; }

        public SelectionDrawer(PictureBox overlayBox, Color transparencyKey)
        {
            _overlayBox = overlayBox;
            _overlayBox.MouseDown += OnMouseDown;
            _overlayBox.MouseMove += OnMouseMove;
            _overlayBox.MouseUp += OnMouseUp;

            _transparencyKey = transparencyKey;
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
                Clear(Selection);
                var newSelection = CalculateRectangle(_startLocation, e.Location);
                Select(newSelection);

                _overlayBox.Invalidate(GetContainingRectangle(Selection, newSelection));
                Selection = newSelection;
            }
        }

        private void Clear(Rectangle rectangle)
        {
            var brush = new SolidBrush(Color.Transparent);
            using (Graphics g = Graphics.FromImage(_overlayBox.Image))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(brush, rectangle);
            }
        }

        private Rectangle CalculateRectangle(Point startLocation, Point endLocation)
        {
            var startX = Math.Min(startLocation.X, endLocation.X);
            var startY = Math.Min(startLocation.Y, endLocation.Y);
            var endX = Math.Max(startLocation.X, endLocation.X);
            var endY = Math.Max(startLocation.Y, endLocation.Y);

            return new Rectangle(new Point(startX, startY), new Size(endX - startX, endY - startY));
        }

        private void Select(Rectangle rectangle)
        {
            var brush = new SolidBrush(_transparencyKey);
            using (Graphics g = Graphics.FromImage(_overlayBox.Image))
            {
                g.FillRectangle(brush, rectangle);
            }
        }

        private Rectangle GetContainingRectangle(Rectangle rectangle1, Rectangle rectangle2)
        {
            var startX = Math.Min(rectangle1.X, rectangle2.X);
            var startY = Math.Min(rectangle1.Y, rectangle2.Y);

            var width = Math.Max(rectangle1.Width, rectangle2.Width);
            var height = Math.Max(rectangle1.Height, rectangle2.Height);

            return new Rectangle(new Point(startX, startY), new Size(width, height));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _choosingRectangle = false;
        }
    }
}