namespace SelfHostedYoloScreenCapture
{
    using System.Windows.Forms;

    public interface MouseEvents
    {
        event MouseEventHandler MouseDown;
        event MouseEventHandler MouseUp;
        event MouseEventHandler MouseMove;
    }
}