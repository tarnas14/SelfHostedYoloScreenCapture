namespace SelfHostedYoloScreenCapture.Painting
{
    using System.Drawing;
    using System.Windows.Forms;

    public class RectangleMouseEvents : MouseEvents
    {
        private readonly Rectangle _rectangle;
        private MouseEventArgs _lastMoveArgs;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;

        public RectangleMouseEvents(Rectangle rectangle, MouseEvents mouseEvents)
        {
            _rectangle = rectangle;
            mouseEvents.MouseDown += FilterMouseDown;
            mouseEvents.MouseMove += FilterMouseMove;
            mouseEvents.MouseUp += FilterMouseUp;
        }

        private void FilterMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown == null)
            {
                return;
            }

            if (InsideRectangle(e))
            {
                MouseDown(sender, e);
            }
        }

        private void FilterMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMove == null)
            {
                return;
            }

            if (InsideRectangle(e))
            {
                _lastMoveArgs = e;
                MouseMove(sender, e);
            }
        }

        private void FilterMouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUp == null)
            {
                return;
            }

            if (InsideRectangle(e))
            {
                MouseUp(sender, e);
                return;
            }

            if (_lastMoveArgs != null)
            {
                MouseUp(sender, _lastMoveArgs);
            }
        }

        private bool InsideRectangle(MouseEventArgs mouseEventArgs)
        {
            var xInsideRectangle = mouseEventArgs.X >= _rectangle.Left && mouseEventArgs.X <= _rectangle.Right;
            var yInsideRectangle = mouseEventArgs.Y >= _rectangle.Top && mouseEventArgs.Y <= _rectangle.Bottom;

            return xInsideRectangle && yInsideRectangle;
        }
    }
}