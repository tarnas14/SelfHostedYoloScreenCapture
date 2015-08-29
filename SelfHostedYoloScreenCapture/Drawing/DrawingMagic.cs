namespace SelfHostedYoloScreenCapture.Drawing
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using SelectingRectangle;

    class DrawingMagic
    {
        private readonly SelectionCanvas _background;
        private readonly SelectionCanvas _foreground;
        private bool _workingMagic;
        private Point _start;
        private Rectangle _lastWorkspace;
        public event EventHandler OperationFinished;

        public DrawingMagic(SelectionCanvas background, SelectionCanvas foreground, MouseEvents mouseEvents)
        {
            _background = background;
            _foreground = foreground;
            mouseEvents.MouseDown += OnMouseDown;
            mouseEvents.MouseMove += OnMouseMove;
            mouseEvents.MouseUp += OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_workingMagic)
            {
                return;
            }

            _workingMagic = true;
            _start = e.Location;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_workingMagic)
            {
                return;
            }

            var lineEnd = e.Location;
            var currentWorkspace = StaticHelper.GetRectangle(_start, lineEnd);

            using (var pen = new Pen(new SolidBrush(Color.Red)))
            using (var canvasGraphics = Graphics.FromImage(_foreground.Canvas))
            {
                canvasGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                var lastWorkspaceRect = StaticHelper.Inflate(_lastWorkspace, 1);
                Clear(canvasGraphics, lastWorkspaceRect);
                canvasGraphics.DrawLine(pen, _start, lineEnd);
            }

            _foreground.Invalidate(StaticHelper.Inflate(StaticHelper.Contain(_lastWorkspace, currentWorkspace), 1));
            _lastWorkspace = currentWorkspace;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!_workingMagic)
            {
                return;
            }

            var lineEnd = e.Location;
            var currentWorkspace = StaticHelper.GetRectangle(_start, lineEnd);

            using (var pen = new Pen(new SolidBrush(Color.Red)))
            using (var backgroundGraphics = Graphics.FromImage(_background.Canvas))
            using (var foregroundGraphics = Graphics.FromImage(_foreground.Canvas))
            {
                Clear(foregroundGraphics, _lastWorkspace);
                backgroundGraphics.DrawLine(pen, _start, lineEnd);
            }

            _foreground.Invalidate(_lastWorkspace);
            _background.Invalidate(StaticHelper.Inflate(currentWorkspace, 1));

            _lastWorkspace = new Rectangle();
            _workingMagic = false;

            if (OperationFinished != null)
            {
                OperationFinished(this, EventArgs.Empty);
            }
        }

        private void Clear(Graphics canvasGraphics, Rectangle rectangle)
        {
            canvasGraphics.FillRectangle(new SolidBrush(Color.Transparent), rectangle);
        }
    }
}
