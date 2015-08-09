namespace SelfHostedYoloScreenCapture.Drawing
{
    using System;
    using SelectingRectangle;

    public interface SelectionProviderThingie
    {
        event EventHandler<RectangleSelectedEventArgs> RectangleSelected;
    }
}
