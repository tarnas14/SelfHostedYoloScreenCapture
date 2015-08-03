namespace SelfHostedYoloScreenCapture.Painting
{
    using System.Drawing;
    using System.Windows.Forms;
    using SelectingRectangle;

    class EmptyBoxDrawer
    {
        private readonly OperationQueue _queue;
        private readonly SelectionCanvas _canvas;
        private bool _on;
        private bool _drawing;

        private Point _start;
        private Point _destination;

        public EmptyBoxDrawer(OperationQueue queue, MouseEvents mouseEvents, SelectionCanvas canvas)
        {
            _queue = queue;
            _canvas = canvas;
            mouseEvents.MouseDown += OnMouseDown;
            mouseEvents.MouseMove += OnMouseMove;
            mouseEvents.MouseUp += OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!_on)
            {
                return;
            }

            if (!_drawing)
            {
                _start = e.Location;
                _drawing = true;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_on)
            {
                return;
            }

            _destination = e.Location;

            DrawBox(StaticHelper.GetRectangle(_start, _destination));
        }

        private void DrawBox(Rectangle rectangle)
        {
            using (var pen = new Pen(Brushes.Red))
            using (var g = Graphics.FromImage(_canvas.Canvas))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.DrawRectangle(pen, rectangle);
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!_on)
            {
                return;
            }

            ExecuteUndoableOperation(StaticHelper.GetRectangle(_start, _destination));
        }

        private void ExecuteUndoableOperation(Rectangle rectangle)
        {
            var drawBoxOperation = new DrawBoxOperation(_canvas, rectangle);
            _queue.Execute(drawBoxOperation);
        }

        public void On()
        {
            _on = true;
        }

        public void Off()
        {
            _on = false;
        }
    }

    internal class DrawBoxOperation : Operation
    {
        private readonly SelectionCanvas _canvas;
        private readonly Rectangle _rectangle;

        public DrawBoxOperation(SelectionCanvas canvas, Rectangle rectangle)
        {
            _canvas = canvas;
            _rectangle = rectangle;
            PrepareUndo();
        }

        private void PrepareUndo()
        {
            throw new System.NotImplementedException();
        }

        public void Do()
        {
            throw new System.NotImplementedException();
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }
    }
}
