namespace SelfHostedYoloScreenCapture.Drawing
{
    using System.Windows.Forms;

    class OnOffMouseEvents : MouseEvents
    {
        public OnOffMouseEvents(MouseEvents decoratedMouseEvents)
        {
            decoratedMouseEvents.MouseDown += (sender, args) =>
            {
                if (!On || MouseDown == null)
                {
                    return;
                }

                MouseDown(sender, args);
            };

            decoratedMouseEvents.MouseMove += (sender, args) =>
            {
                if (!On || MouseMove == null)
                {
                    return;
                }

                MouseMove(sender, args);
            };

            decoratedMouseEvents.MouseUp += (sender, args) =>
            {
                if (!On || MouseUp == null)
                {
                    return;
                }

                MouseUp(sender, args);
            };
        }

        public bool On { get; set; }

        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;
    }
}
