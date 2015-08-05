namespace SelfHostedYoloScreenCapture
{
    using System;
    using Painting;

    public class ToolSelectedEventArgs : EventArgs
    {
        public Tool Tool { get; set; }
    }
}