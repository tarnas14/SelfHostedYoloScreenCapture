namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System;
    using System.Drawing;

    public class RectangleSelectedEventArgs : EventArgs
    {
        public Rectangle Selection { get; set; }
        public Size CanvasSize { get; set; }
    }
}