namespace Tests.MouseEventsTests
{
    using System.Windows.Forms;
    using NUnit.Framework;
    using SelfHostedYoloScreenCapture;

    class ChainOfResponsibilityTests
    {
        private MouseEventArgs _insideRectangle;
        private MouseEventArgs _outsideRectangle;
        private object _sender;

        [SetUp]
        public void Setup()
        {
            _sender = new object();
            _insideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 11, 11, 0);
            _outsideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 24, 13, 0);
        }

        [Test]
        public void DumbLink_ShouldJustInvokeEvents()
        {
            //given
            var dumbLink = new DumbMouseEventLink();
            var mouseEventsTester = new MouseEventsTester(dumbLink);

            //when
            dumbLink.DoMouseDown(_sender, _insideRectangle);
            dumbLink.DoMouseDown(_sender, _outsideRectangle);
            dumbLink.DoMouseMove(_sender, _insideRectangle);
            dumbLink.DoMouseMove(_sender, _outsideRectangle);
            dumbLink.DoMouseUp(_sender, _insideRectangle);
            dumbLink.DoMouseUp(_sender, _outsideRectangle);

            //then
            Assert.That(mouseEventsTester.MouseDownArgs.Count, Is.EqualTo(2));
            Assert.That(mouseEventsTester.MouseDownArgs.Contains(_insideRectangle));
            Assert.That(mouseEventsTester.MouseDownArgs.Contains(_outsideRectangle));


            Assert.That(mouseEventsTester.MouseMoveArgs.Count, Is.EqualTo(2));
            Assert.That(mouseEventsTester.MouseMoveArgs.Contains(_insideRectangle));
            Assert.That(mouseEventsTester.MouseMoveArgs.Contains(_outsideRectangle));

            Assert.That(mouseEventsTester.MouseUpArgs.Count, Is.EqualTo(2));
            Assert.That(mouseEventsTester.MouseUpArgs.Contains(_insideRectangle));
            Assert.That(mouseEventsTester.MouseUpArgs.Contains(_outsideRectangle));
        }
    }

    internal class DumbMouseEventLink : MouseEvents
    {
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;

        public void DoMouseDown(object sender, MouseEventArgs args)
        {
            if (MouseDown == null)
            {
                return;
            }

            MouseDown(sender, args);
        }

        public void DoMouseMove(object sender, MouseEventArgs args)
        {
            if (MouseMove == null)
            {
                return;
            }

            MouseMove(sender, args);
        }

        public void DoMouseUp(object sender, MouseEventArgs args)
        {
            if (MouseUp == null)
            {
                return;
            }

            MouseUp(sender, args);
        }
    }
}
