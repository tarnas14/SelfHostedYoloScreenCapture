namespace SelfHostedYoloScreenCapture
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
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
            var stream = new MemoryStream();
            capturedSelection.Save(stream, ImageFormat.Jpeg);
            var streamContent = new ByteArrayContent(stream.ToArray());
            streamContent.Headers.Add("Content-Type", "image/jpeg");

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(streamContent, "upload", "tmp.jpg");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _serverPath);
            requestMessage.Content = multipartFormDataContent;

            _client.SendAsync(requestMessage);
        }
    }
}