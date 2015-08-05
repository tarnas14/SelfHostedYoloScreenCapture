namespace SelfHostedYoloScreenCapture.Painting
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using SelectingRectangle;

    class DrawingMagic
    {
        private readonly SelectionCanvas _canvas;
        private Image _cache;
        private Tool _currentTool;
        private bool _workingMagic;
        private Point _start;
        private readonly OperationQueue _operationQueue;
        public event EventHandler OperationFinished;

        public DrawingMagic(SelectionCanvas canvas, MouseEvents mouseEvents, OperationQueue operationQueue)
        {
            _canvas = canvas;
            _operationQueue = operationQueue;
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
            _currentTool.DrawOn(_canvas, _start, e.Location);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!_workingMagic)
            {
                return;
            }

            _workingMagic = false;
            _operationQueue.Execute(_currentTool.GetUndoableOperation(_cache, _start, e.Location));
            CommitCache();

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
            _currentTool = new NullTool();
            using (var g = Graphics.FromImage(_cache))
            {
                var canvasRectangle = new Rectangle(new Point(0,0), _canvas.Canvas.Size);
                g.DrawImage(_canvas.Canvas, canvasRectangle, canvasRectangle, GraphicsUnit.Pixel);
            }
        }

        public void SetTool(Tool tool)
        {
            if (_workingMagic)
            {
                return;
            }

            _currentTool = tool;
        }
    }
}
