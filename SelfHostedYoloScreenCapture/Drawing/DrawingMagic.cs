namespace SelfHostedYoloScreenCapture.Drawing
{
    using System;
    using System.Windows.Forms;
    using SelectingRectangle;
    using Tools;

    class DrawingMagic
    {
        private readonly SelectionCanvas _background;
        private readonly SelectionCanvas _foreground;
        private Tool _tool;

        public event EventHandler OperationFinished;

        public DrawingMagic(SelectionCanvas background, SelectionCanvas foreground, MouseEvents mouseEvents)
        {
            _background = background;
            _foreground = foreground;
            mouseEvents.MouseDown += OnMouseDown;
            mouseEvents.MouseMove += OnMouseMove;
            mouseEvents.MouseUp += OnMouseUp;

            _tool = new NullTool();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _tool.MouseDown(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _tool.MouseMove(e, _foreground);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _tool.MouseUp(e, _foreground, _background);

            if (OperationFinished != null)
            {
                OperationFinished(this, EventArgs.Empty);
            }
            _tool = new NullTool();
        }

        public void UseTool(object sender, ToolSelectedEventArgs e)
        {
            _tool = e.Tool;
        }
    }
}
