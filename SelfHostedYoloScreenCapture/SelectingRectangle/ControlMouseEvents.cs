namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System.Windows.Forms;

    internal class ControlMouseEvents : MouseEvents
    {
        private readonly PictureBox _pictureBox;

        public ControlMouseEvents(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;
        }

        public event MouseEventHandler MouseDown
        {
            add { _pictureBox.MouseDown += value; }
            remove { _pictureBox.MouseDown -= value; }
        }

        public event MouseEventHandler MouseUp
        {
            add { _pictureBox.MouseUp += value; }
            remove { _pictureBox.MouseUp -= value; }
        }

        public event MouseEventHandler MouseMove
        {
            add { _pictureBox.MouseMove += value; }
            remove { _pictureBox.MouseMove -= value; }
        }
    }
}