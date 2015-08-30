namespace SelfHostedYoloScreenCapture
{
    using System;
    using Drawing.Tools;

    public class ToolSelectedEventArgs : EventArgs
    {
        public Tool Tool { get; set; }
    }
}