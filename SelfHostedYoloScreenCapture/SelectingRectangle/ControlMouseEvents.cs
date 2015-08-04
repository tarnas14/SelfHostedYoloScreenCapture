namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System.Windows.Forms;

    internal class ControlMouseEvents : MouseEvents
    {
        private readonly Control _control;

        public ControlMouseEvents(Control control)
        {
            _control = control;
        }

        public event MouseEventHandler MouseDown
        {
            add { _control.MouseDown += value; }
            remove { _control.MouseDown -= value; }
        }

        public event MouseEventHandler MouseUp
        {
            add { _control.MouseUp += value; }
            remove { _control.MouseUp -= value; }
        }

        public event MouseEventHandler MouseMove
        {
            add { _control.MouseMove += value; }
            remove { _control.MouseMove -= value; }
        }
    }
}