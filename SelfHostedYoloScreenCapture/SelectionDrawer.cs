namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SelectionDrawer
    {
        private bool _selecting;
        private Point _startLocation;
        private readonly SolidBrush _selectionBrush;
        private readonly SelectionCanvas _selectionCanvas;

        public Rectangle Selection { get; private set; }

        public SelectionDrawer(SelectionCanvas selectionCanvas, MouseEvents mouseEvents, Color selectionColour)
        {
            _selectionBrush = new SolidBrush(selectionColour);
            _selectionCanvas = selectionCanvas;
            mouseEvents.MouseDown += OnMouseDown;
            mouseEvents.MouseMove += OnMouseMove;
            mouseEvents.MouseUp += OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _selecting = true;
            _startLocation = e.Location;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //if (_selecting)
            //{
            //    Clear(Selection);
            //    var newSelection = CalculateRectangle(_startLocation, e.Location);
            //    Select(newSelection);

            //    _overlayBox.Invalidate(GetContainingRectangle(Selection, newSelection));
            //    Selection = newSelection;
            //}
        }

        private void Clear(Rectangle rectangle)
        {
            var brush = new SolidBrush(Color.Transparent);
            using (Graphics g = Graphics.FromImage(_selectionCanvas.Canvas))
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
            using (Graphics g = Graphics.FromImage(_selectionCanvas.Canvas))
            {
                g.FillRectangle(_selectionBrush, rectangle);
            }
        }

        private Rectangle GetContainingRectangle(Rectangle rectangle1, Rectangle rectangle2)
        {
            var startX = Math.Min(rectangle1.X, rectangle2.X);
            var startY = Math.Min(rectangle1.Y, rectangle2.Y);

            var endX = Math.Max(rectangle1.Right, rectangle2.Right);
            var endY = Math.Max(rectangle1.Bottom, rectangle2.Bottom);

            return new Rectangle(new Point(startX, startY), new Size(endX - startX, endY - startY));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_selecting)
            {
                Clear(Selection);
                var newSelection = CalculateRectangle(_startLocation, e.Location);
                Select(newSelection);

                _selectionCanvas.InvalidateCanvas(GetContainingRectangle(Selection, newSelection));
                Selection = newSelection;
            }
            _selecting = false;
        }
    }
}