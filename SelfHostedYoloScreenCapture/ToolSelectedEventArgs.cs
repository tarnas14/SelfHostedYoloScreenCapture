namespace SelfHostedYoloScreenCapture
{
    using System;
    using Drawing;

    public class ToolSelectedEventArgs : EventArgs
    {
        public Tool Tool { get; set; }
    }
}