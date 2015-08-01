namespace Tests.PaintingTests
{
    using System.Windows.Forms;
    using SelfHostedYoloScreenCapture;
    using SelfHostedYoloScreenCapture.Painting;

    class MouseEventsMock : MouseEvents
    {
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;

        public void DoMouseUp(MouseEventArgs args)
        {
            MouseUp(this, args);
        }

        public void DoMouseDown(MouseEventArgs args)
        {
            MouseDown(this, args);
        }

        public void DoMouseMove(MouseEventArgs args)
        {
            MouseMove(this, args);
        }
    }
}
