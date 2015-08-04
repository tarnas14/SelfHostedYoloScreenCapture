namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System.Drawing;

    public interface SelectionCanvas
    {
        Image Canvas { get; }
        void Invalidate();
        void Invalidate(Rectangle rectangle);
    }
}