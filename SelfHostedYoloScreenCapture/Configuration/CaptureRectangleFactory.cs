namespace SelfHostedYoloScreenCapture.Configuration
{
    using System.Drawing;

    public interface CaptureRectangleFactory
    {
        Rectangle GetRectangle();
    }
}