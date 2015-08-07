namespace Tests.MouseEventsTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;
    using SelfHostedYoloScreenCapture;

    class MouseEventsTester
    {
        public MouseEventsTester(MouseEvents mousEevents)
        {
            MouseUpArgs = new Collection<MouseEventArgs>();
            MouseDownArgs = new Collection<MouseEventArgs>();
            MouseMoveArgs = new Collection<MouseEventArgs>();

            mousEevents.MouseDown += (caller, args) => MouseDownArgs.Add(args);
            mousEevents.MouseUp += (caller, args) => MouseUpArgs.Add(args);
            mousEevents.MouseMove += (caller, args) => MouseMoveArgs.Add(args);
        }

        public ICollection<MouseEventArgs> MouseUpArgs { get; private set; }
        public ICollection<MouseEventArgs> MouseDownArgs { get; private set; }
        public ICollection<MouseEventArgs> MouseMoveArgs { get; private set; }
    }
}
