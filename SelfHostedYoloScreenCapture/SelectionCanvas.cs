namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;

    public interface SelectionCanvas
    {
        Image Canvas { get; }
        void InvalidateCanvas(Rectangle rectangle);
    }
}