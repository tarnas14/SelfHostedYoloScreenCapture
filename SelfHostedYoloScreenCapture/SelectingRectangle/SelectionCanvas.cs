namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System.Drawing;
    using Painting;

    public interface SelectionCanvas : MouseEvents
    {
        Image Canvas { get; }
        void Invalidate(Rectangle rectangle);
    }
}