namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    using System.Drawing;
    using System.Net.Http;
    using System.Windows.Forms;
    using Newtonsoft.Json;

    public partial class PhotoUploaderPresentingResult : Form, PhotoUploader
    {
        private readonly string _serverPath;

        public PhotoUploaderPresentingResult(string serverPath)
        {
            _serverPath = serverPath;
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
            var httpRequestMessage = RequestFactory.GetMessage(capturedSelection, _serverPath);

            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;

                var resultString = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<ServerResult>(resultString);

                _path.Text = result.ImagePath;
                _progressBar.Visible = false;
            }
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
            public string ImagePath { get; set; }
        }
    }
}
