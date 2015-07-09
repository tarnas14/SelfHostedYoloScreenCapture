namespace SelfHostedYoloScreenCapture.PhotoUploading
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net.Http;

    static class RequestFactory
    {
        private const string PngMime = "image/png";

        public static HttpRequestMessage GetMessage(Image image, string serverPath)
        {
            var stream = new MemoryStream();
            EncoderParameters encparams = new EncoderParameters(1);
            encparams.Param[0] = new EncoderParameter(Encoder.Quality, 97L);

            image.Save(stream, GetEncoderInfo(PngMime), encparams);
            var streamContent = new ByteArrayContent(stream.ToArray());
            streamContent.Headers.Add("Content-Type", PngMime);

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(streamContent, "upload", "tmp.png");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, serverPath);
            requestMessage.Content = multipartFormDataContent;

            return requestMessage;
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            return codecs.FirstOrDefault(codec => codec.MimeType == mimeType);
        }
    }
}
