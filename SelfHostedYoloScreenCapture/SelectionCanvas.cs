namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;
    using System.Windows.Forms;

    public interface SelectionCanvas
    {
        event MouseEventHandler MouseDown;
        event MouseEventHandler MouseUp;
        event MouseEventHandler MouseMove;
        Image Canvas { get; }
        void Invalidate(Rectangle rectangle);
    }
}