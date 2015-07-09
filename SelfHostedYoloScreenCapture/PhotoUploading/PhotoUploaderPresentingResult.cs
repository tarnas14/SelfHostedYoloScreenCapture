namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    using System.Drawing;
    using System.Net.Http;
    using System.Windows.Forms;
    using Newtonsoft.Json;

    public partial class PhotoUploaderPresentingResult : Form, PhotoUploader
    {
        private readonly string _uploadPath;
        private readonly string _serverGetPicturePath;

        public PhotoUploaderPresentingResult(string uploadPath, string serverGetPicturePath)
        {
            _uploadPath = uploadPath;
            _serverGetPicturePath = serverGetPicturePath;
            InitializeComponent();
            _progressBar.Style = ProgressBarStyle.Marquee;
            _progressBar.MarqueeAnimationSpeed = 30;

            _close.Click += (sender, args) => Hide();
            _copy.Click += (sender, args) =>
            {
                Clipboard.SetText(_path.Text);
                Hide();
            };
        }

        public void Upload(Image capturedSelection)
        {
            ShowUploader();
            var httpRequestMessage = RequestFactory.GetMessage(capturedSelection, _uploadPath);

            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;

                var resultString = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<ServerResult>(resultString);

                _path.Text = PutPictureGetPathTogether(_serverGetPicturePath, result.ImageName);
                _progressBar.Visible = false;
            }
        }

        private string PutPictureGetPathTogether(string serverGetPicturePath, string imageName)
        {
            var slashAtTheEndOfPath = serverGetPicturePath.EndsWith("/") ? string.Empty : "/";
            return string.Format("{0}{1}{2}", serverGetPicturePath, slashAtTheEndOfPath, imageName);
        }

        private void ShowUploader()
        {
            _progressBar.Visible = true;
            Show();
            BringToFront();
            Activate();
        }

        struct ServerResult
        {
            public string ImageName { get; set; }
        }
    }
}
