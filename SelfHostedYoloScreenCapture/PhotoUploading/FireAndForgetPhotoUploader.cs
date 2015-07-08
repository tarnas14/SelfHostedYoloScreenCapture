namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    using System.Drawing;
    using System.Net.Http;

    public class FireAndForgetPhotoUploader : PhotoUploader
    {
        private readonly string _serverPath;
        private readonly HttpClient _client;

        public FireAndForgetPhotoUploader(string serverPath)
        {
            _serverPath = serverPath; 
            _client = new HttpClient();
        }

        public void Upload(Image capturedSelection)
        {
            var requestMessage = RequestFactory.GetMessage(capturedSelection, _serverPath);

            _client.SendAsync(requestMessage);
        }
    }
}