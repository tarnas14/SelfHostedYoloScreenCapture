namespace SelfHostedYoloScreenCapture.Painting
{
    using System;
    using SelectingRectangle;

    public interface SelectionProviderThingie
    {
        event EventHandler<RectangleSelectedEventArgs> RectangleSelected;
    }
}
