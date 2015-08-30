namespace SelfHostedYoloScreenCapture.Drawing.Tools
{
    using System.Windows.Forms;
    using SelectingRectangle;

    public interface Tool
    {
        void MouseDown(MouseEventArgs mouseEventArgs);
        void MouseMove(MouseEventArgs mouseEventArgs, SelectionCanvas foreground);
        void MouseUp(MouseEventArgs mouseEventArgs, SelectionCanvas foreground, SelectionCanvas background);
    }
}