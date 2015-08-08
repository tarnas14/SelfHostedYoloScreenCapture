namespace SelfHostedYoloScreenCapture.Painting
{
    using System.Windows.Forms;

    public interface ResponsibilityChainLink
    {
        void DoMouseDown(object sender, MouseEventArgs args);
        void DoMouseMove(object sender, MouseEventArgs args);
        void DoMouseUp(object sender, MouseEventArgs args);
    }
}