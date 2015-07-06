namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;

    public class PhotoUploader
    {
        private readonly string _serverPath;

        public PhotoUploader(string serverPath)
        {
            _serverPath = serverPath;
        }

        public void Upload(Image capturedSelection)
        {
            using (var client = new HttpClient())
            {
                var stream = new MemoryStream();
                capturedSelection.Save(stream, ImageFormat.Jpeg);
                var streamContent = new ByteArrayContent(stream.ToArray());
                streamContent.Headers.Add("Content-Type", "image/jpeg");

                var multipartFormDataContent = new MultipartFormDataContent();
                multipartFormDataContent.Add(streamContent, "upload", "tmp.jpg");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, _serverPath);
                requestMessage.Content = multipartFormDataContent;

                var httpResponseMessage = client.SendAsync(requestMessage).Result;

                var resultString = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var resultObject = JsonConvert.DeserializeObject<ServerResult>(resultString);

                Console.WriteLine(resultObject.ImagePath);
            }
        }

        class ServerResult
        {
            public string ImagePath { get; set; }
        }
    }
}