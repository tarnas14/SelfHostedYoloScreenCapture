namespace SelfHostedYoloScreenCapture.Drawing.Tools
{
    using System.Drawing;
    using System.Windows.Forms;
    using SelectingRectangle;

    class DrawLine : Tool
    {
        private bool _workingMagic;
        private Point _start;
        private Rectangle _lastWorkspace;

        public void MouseDown(MouseEventArgs mouseEventArgs)
        {
            if (_workingMagic)
            {
                return;
            }

            _workingMagic = true;
            _start = mouseEventArgs.Location;
        }

        public void MouseMove(MouseEventArgs mouseEventArgs, SelectionCanvas foreground)
        {
            if (!_workingMagic)
            {
                return;
            }

            var lineEnd = mouseEventArgs.Location;
            var currentWorkspace = StaticHelper.GetRectangle(_start, lineEnd);

            using (var pen = new Pen(new SolidBrush(Color.Red)))
            using (var canvasGraphics = Graphics.FromImage(foreground.Canvas))
            {
                canvasGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                var lastWorkspaceRect = StaticHelper.Inflate(_lastWorkspace, 1);
                StaticHelper.Clear(canvasGraphics, lastWorkspaceRect);
                canvasGraphics.DrawLine(pen, _start, lineEnd);
            }

            foreground.Invalidate(StaticHelper.Inflate(StaticHelper.Contain(_lastWorkspace, currentWorkspace), 1));
            _lastWorkspace = currentWorkspace;
        }

        public void MouseUp(MouseEventArgs mouseEventArgs, SelectionCanvas foreground, SelectionCanvas background)
        {
            if (!_workingMagic)
            {
                return;
            }

            var lineEnd = mouseEventArgs.Location;
            var currentWorkspace = StaticHelper.GetRectangle(_start, lineEnd);

            using (var pen = new Pen(new SolidBrush(Color.Red)))
            using (var backgroundGraphics = Graphics.FromImage(background.Canvas))
            using (var foregroundGraphics = Graphics.FromImage(foreground.Canvas))
            {
                StaticHelper.Clear(foregroundGraphics, _lastWorkspace);
                backgroundGraphics.DrawLine(pen, _start, lineEnd);
            }

            foreground.Invalidate(_lastWorkspace);
            background.Invalidate(StaticHelper.Inflate(currentWorkspace, 1));

            _lastWorkspace = new Rectangle();
            _workingMagic = false;
        }
    }
}
