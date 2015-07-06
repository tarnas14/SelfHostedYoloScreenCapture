namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    using System.Drawing;
    using System.Windows.Forms;

    public partial class PhotoUploaderPresentingResult : Form, PhotoUploader
    {
        public PhotoUploaderPresentingResult()
        {
            InitializeComponent();
        }

        public void Upload(Image capturedSelection)
        {
            
        }
    }
}
