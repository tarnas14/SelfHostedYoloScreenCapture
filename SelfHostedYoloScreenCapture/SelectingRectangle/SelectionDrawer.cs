namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Drawing;

    public class SelectionDrawer : SelectionProviderThingie
    {
        private bool _selecting;
        private Point _startLocation;
        private readonly SelectionCanvas _selectionCanvas;
        private SolidBrush _overlayBrush;

        public event EventHandler<RectangleSelectedEventArgs> RectangleSelected;
        public event EventHandler NewSelectionStarted;

        public Rectangle Selection { get; private set; }

        public SelectionDrawer(SelectionCanvas selectionCanvas, MouseEvents mouseEvents)
        {
            _selectionCanvas = selectionCanvas;
            mouseEvents.MouseDown += OnMouseDown;
            mouseEvents.MouseMove += OnMouseMove;
            mouseEvents.MouseUp += OnMouseUp;

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

                _selectionCanvas.Invalidate(StaticHelper.Contain(Selection, newSelection));
                Selection = RestrictToCanvas(newSelection, _selectionCanvas.Canvas.Size);
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

        private Rectangle RestrictToCanvas(Rectangle selection, Size size)
        {
            var x = selection.X < 0 ? 0 : selection.X;
            var y = selection.Y < 0 ? 0 : selection.Y;

            var bottom = selection.Bottom < size.Height ? selection.Bottom : size.Height;
            var right = selection.Right < size.Width ? selection.Right : size.Width;

            return new Rectangle(new Point(x, y), new Size(right - x, bottom - y));
        }

        public void ResetSelection()
        {
            PrepareOverlay();
        }
    }
}