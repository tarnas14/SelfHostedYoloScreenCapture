namespace SelfHostedYoloScreenCapture.Drawing.Tools
{
    using System.Windows.Forms;
    using SelectingRectangle;

    class NullTool : Tool
    {
        public void MouseDown(MouseEventArgs mouseEventArgs)
        {
            //nothing here, move along
        }

        public void MouseMove(MouseEventArgs mouseEventArgs, SelectionCanvas foreground)
        {
            //nothing here, move along
        }

        public void MouseUp(MouseEventArgs mouseEventArgs, SelectionCanvas foreground, SelectionCanvas background)
        {
            //nothing here, move along
        }
    }
}
