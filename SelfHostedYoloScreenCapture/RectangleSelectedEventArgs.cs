namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;

    public class RectangleSelectedEventArgs : EventArgs
    {
        public Rectangle Selection { get; set; }
    }
}