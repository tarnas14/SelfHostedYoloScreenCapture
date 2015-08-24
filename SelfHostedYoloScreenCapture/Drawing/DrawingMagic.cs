namespace SelfHostedYoloScreenCapture.Drawing
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using SelectingRectangle;

    class DrawingMagic
    {
        private readonly SelectionCanvas _canvas;
        private Image _cache;
        private bool _workingMagic;
        private Point _start;
        private Rectangle _lastWorkspace;
        public event EventHandler OperationFinished;

        public DrawingMagic(SelectionCanvas canvas, MouseEvents mouseEvents)
        {
            _canvas = canvas;
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
            var workspaceRectangle = StaticHelper.GetRectangle(_start, lineEnd);

            using (var pen = new Pen(new SolidBrush(Color.Red)))
            using (var canvasGraphics = Graphics.FromImage(_canvas.Canvas))
            {
                var destRect = StaticHelper.Inflate(StaticHelper.Contain(workspaceRectangle, _lastWorkspace), 1);
                canvasGraphics.DrawImage(_cache, destRect, destRect, GraphicsUnit.Pixel);
                canvasGraphics.DrawLine(pen, _start, lineEnd);
                _canvas.Invalidate();
            }

            _lastWorkspace = workspaceRectangle;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!_workingMagic)
            {
                return;
            }

            var lineEnd = e.Location;

            using (var pen = new Pen(new SolidBrush(Color.Red)))
            using (var cacheGraphics = Graphics.FromImage(_cache))
            {
                cacheGraphics.DrawImage(_cache, new Rectangle(Point.Empty, _cache.Size));
                cacheGraphics.DrawLine(pen, _start, lineEnd);
                _canvas.Invalidate();
            }

            CommitCache();

            _lastWorkspace = new Rectangle();
            _workingMagic = false;

            if (OperationFinished != null)
            {
                OperationFinished(this, EventArgs.Empty);
            }
        }

        private void CommitCache()
        {
            using (var g = Graphics.FromImage(_canvas.Canvas))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(new Point(0,0), _canvas.Canvas.Size));
                g.DrawImage(_cache, new Point(0,0));
                _canvas.Invalidate();
            }
        }

        public void UpdateCache(object sender, RectangleSelectedEventArgs rectangleSelectedArgs)
        {
            _cache = new Bitmap(_canvas.Canvas.Width, _canvas.Canvas.Height);
            using (var g = Graphics.FromImage(_cache))
            {
                var canvasRectangle = new Rectangle(new Point(0,0), _canvas.Canvas.Size);
                g.DrawImage(_canvas.Canvas, canvasRectangle, canvasRectangle, GraphicsUnit.Pixel);
            }
        }
    }
}
