namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    using System.Drawing;

    public interface PhotoUploader
    {
        void Upload(Image capturedSelection);
    }
}