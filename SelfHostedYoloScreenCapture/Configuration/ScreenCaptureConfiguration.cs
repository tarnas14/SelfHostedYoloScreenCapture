namespace SelfHostedYoloScreenCapture.Configuration
{
    using System.Drawing;

    class ScreenCaptureConfiguration : Configuration<ScreenCaptureConfiguration>
    {
        public bool Fullscreen { get; set; }
        public Rectangle CustomCaptureRectangle { get; set; }

        public string ServerPath { get; set; }

        public ScreenCaptureConfiguration Default
        {
            get
            {
                return new ScreenCaptureConfiguration
                {
                    Fullscreen = true,
                    CustomCaptureRectangle = new Rectangle(new Point(0, 0), new Size(800, 600)),
                    ServerPath = "http://localhost.fiddler:4000/upload"
                };
            }
        }
    }
}
