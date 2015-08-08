namespace SelfHostedYoloScreenCapture.Painting
{
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolDrawingInsideSelectionChainLink : MouseEvents
    {
        private readonly ResponsibilityChainLink _nextLink;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;
        private Rectangle _rectangle;
        private bool _isMouseDownInsideRectangle;
        private MouseEventArgs _lastMoveArgs;

        public ToolDrawingInsideSelectionChainLink(SelectionProviderThingie selectionProviderThingie, ResponsibilityChainLink nextLink)
        {
            _nextLink = nextLink;
            selectionProviderThingie.RectangleSelected += (sender, args) => { _rectangle = args.Selection; };
            selectionProviderThingie.NewSelectionStarted += (sender, args) =>
            {
                _rectangle = new Rectangle();
                _isMouseDownInsideRectangle = false;
                _lastMoveArgs = null;
            };
        }

        public void DoMouseDown(object sender, MouseEventArgs args)
        {
            if (!InsideRectangle(args))
            {
                _nextLink.DoMouseDown(sender, args);
                return;
            }

            if (MouseDown == null)
            {
                return;
            }

            MouseDown(sender, args);
            _isMouseDownInsideRectangle = true;
        }

        public void DoMouseMove(object sender, MouseEventArgs args)
        {
            if (!InsideRectangle(args))
            {
                _nextLink.DoMouseMove(sender, args);
                return;
            }

            if (MouseMove == null)
            {
                return;
            }

            _lastMoveArgs = args;
            MouseMove(sender, args);
        }

        public void DoMouseUp(object sender, MouseEventArgs args)
        {
            if (MouseUp == null)
            {
                return;
            }

            if (InsideRectangle(args))
            {
                MouseUp(sender, args);
                return;
            }

            if (_isMouseDownInsideRectangle && _lastMoveArgs != null && !InsideRectangle(args))
            {
                MouseUp(sender, _lastMoveArgs);
                _isMouseDownInsideRectangle = false;
                _lastMoveArgs = null;
                return;
            }

            _nextLink.DoMouseUp(sender, args);
        }

        private bool InsideRectangle(MouseEventArgs mouseEventArgs)
        {
            var xInsideRectangle = mouseEventArgs.X >= _rectangle.Left && mouseEventArgs.X <= _rectangle.Right;
            var yInsideRectangle = mouseEventArgs.Y >= _rectangle.Top && mouseEventArgs.Y <= _rectangle.Bottom;

            return xInsideRectangle && yInsideRectangle;
        }
    }
}