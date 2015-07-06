namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;

    public interface PhotoUploader
    {
        void Upload(Image capturedSelection);
    }
}