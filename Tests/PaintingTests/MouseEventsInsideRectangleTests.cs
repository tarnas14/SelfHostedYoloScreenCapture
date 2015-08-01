namespace Tests.PaintingTests
{
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using NUnit.Framework;
    using SelfHostedYoloScreenCapture.Painting;

    class MouseEventsInsideRectangleTests
    {
        private MouseEventsMock _unfilteredMouseEvents;
        private Rectangle _rectangle;
        private MouseEventsTester _rectangleEventsTester;

        [SetUp]
        public void Setup()
        {
            _unfilteredMouseEvents = new MouseEventsMock();
            _rectangle = new Rectangle(new Point(10, 10), new Size(10, 10));
            var rectangleMouseEvents = new RectangleMouseEvents(_rectangle, _unfilteredMouseEvents);
            _rectangleEventsTester = new MouseEventsTester(rectangleMouseEvents);
        }

        [Test]
        public void ShouldPassMouseEventsInsideRectangleUnchanged()
        {
            //given
            var mouseDownArgsInsideRectangle = new MouseEventArgs(MouseButtons.Left,  1, 11, 11, 0);
            var mouseMoveArgsInsideRectangle = new MouseEventArgs(MouseButtons.Left, 1,  13,  13,  0);
            var mouseUpArgsInsideRectangle = new MouseEventArgs(MouseButtons.Left, 1,  12, 12, 0);

            //when
            _unfilteredMouseEvents.DoMouseDown(mouseDownArgsInsideRectangle);
            _unfilteredMouseEvents.DoMouseMove(mouseMoveArgsInsideRectangle);
            _unfilteredMouseEvents.DoMouseUp(mouseUpArgsInsideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseDownArgs.Single(), Is.EqualTo(mouseDownArgsInsideRectangle));
            Assert.That(_rectangleEventsTester.MouseMoveArgs.Single(), Is.EqualTo(mouseMoveArgsInsideRectangle));
            Assert.That(_rectangleEventsTester.MouseUpArgs.Single(), Is.EqualTo(mouseUpArgsInsideRectangle));
        }

        [Test]
        public void ShouldNotPassMouseEventsOutsideRectangle()
        {
            //given
            var mouseDownArgsOutsideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 8, 10, 0);
            var mouseMoveArgsOutsideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 24, 13, 0);
            var mouseUpArgsOutsideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 44, 3, 0);

            //when
            _unfilteredMouseEvents.DoMouseDown(mouseDownArgsOutsideRectangle);
            _unfilteredMouseEvents.DoMouseMove(mouseMoveArgsOutsideRectangle);
            _unfilteredMouseEvents.DoMouseUp(mouseUpArgsOutsideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseDownArgs, Is.Empty);
            Assert.That(_rectangleEventsTester.MouseMoveArgs, Is.Empty);
            Assert.That(_rectangleEventsTester.MouseUpArgs, Is.Empty);
        }
    }
}