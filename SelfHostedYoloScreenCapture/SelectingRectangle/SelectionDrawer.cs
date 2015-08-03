namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Painting;

    public class SelectionDrawer
    {
        private bool _selecting;
        private Point _startLocation;
        private readonly SelectionCanvas _selectionCanvas;
        private SolidBrush _overlayBrush;

        public event EventHandler<RectangleSelectedEventArgs> RectangleSelected;
        public event EventHandler NewSelectionStarted;

        public Rectangle Selection { get; private set; }

        public SelectionDrawer(SelectionCanvas selectionCanvas)
        {
            _selectionCanvas = selectionCanvas;
            selectionCanvas.MouseDown += OnMouseDown;
            selectionCanvas.MouseMove += OnMouseMove;
            selectionCanvas.MouseUp += OnMouseUp;

            PrepareOverlay();
        }

        private void PrepareOverlay()
        {
            _overlayBrush = new SolidBrush(Color.FromArgb(100, Color.Black));
            using (var graphics = Graphics.FromImage(_selectionCanvas.Canvas))
            {
                Selection = new Rectangle();
                graphics.Clear(Color.Transparent);
                graphics.FillRectangle(_overlayBrush, new Rectangle(Point.Empty, _selectionCanvas.Canvas.Size));
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _selecting = true;
            _startLocation = e.Location;
            if (NewSelectionStarted != null)
            {
                NewSelectionStarted(this, EventArgs.Empty);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_selecting)
            {
                var newSelection = StaticHelper.GetRectangle(_startLocation, e.Location);

                if (newSelection == Selection)
                {
                    return;
                }

                Clear(Selection);
                Select(newSelection);

                _selectionCanvas.Invalidate(GetContainingRectangle(Selection, newSelection));
                Selection = newSelection;
            }
        }

        private void Clear(Rectangle rectangle)
        {
            using (Graphics g = Graphics.FromImage(_selectionCanvas.Canvas))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(_overlayBrush, rectangle);
            }
        }

        private void Select(Rectangle rectangle)
        {
            using (var transparentBrush = new SolidBrush(Color.Transparent))
            using (Graphics g = Graphics.FromImage(_selectionCanvas.Canvas))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(transparentBrush, rectangle);
                ControlPaint.DrawBorder(g, rectangle, Color.Linen, ButtonBorderStyle.Dashed);
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
            _selecting = false;
            if (RectangleSelected != null)
            {
                RectangleSelected(this, new RectangleSelectedEventArgs
                {
                    Selection = Selection,
                    CanvasSize = _selectionCanvas.Canvas.Size
                });
            }
        }

        public void ResetSelection()
        {
            PrepareOverlay();
        }
    }
}