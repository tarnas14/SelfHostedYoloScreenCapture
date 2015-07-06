namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net.Http;

    static class RequestFactory
    {
        public static HttpRequestMessage GetMessage(Image image, string serverPath)
        {
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            var streamContent = new ByteArrayContent(stream.ToArray());
            streamContent.Headers.Add("Content-Type", "image/jpeg");

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(streamContent, "upload", "tmp.jpg");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, serverPath);
            requestMessage.Content = multipartFormDataContent;

            return requestMessage;
        }
    }
}
