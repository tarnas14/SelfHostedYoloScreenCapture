namespace Tests.PaintingTests
{
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using FakeItEasy;
    using NUnit.Framework;
    using SelfHostedYoloScreenCapture.Painting;
    using SelfHostedYoloScreenCapture.SelectingRectangle;

    class MouseEventsInsideRectangleTests
    {
        private MouseEventsMock _unfilteredMouseEvents;
        private MouseEventsTester _rectangleEventsTester;
        private MouseEventArgs _insideRectangle;
        private MouseEventArgs _outsideRectangle;

        [SetUp]
        public void Setup()
        {
            _unfilteredMouseEvents = new MouseEventsMock();
            _insideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 11, 11, 0);
            _outsideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 24, 13, 0);
            var selectionProvider = A.Fake<SelectionProviderThingie>();
            var rectangleMouseEvents = new RectangleMouseEvents(selectionProvider, _unfilteredMouseEvents);
            _rectangleEventsTester = new MouseEventsTester(rectangleMouseEvents);
            selectionProvider.RectangleSelected += Raise.With(this, new RectangleSelectedEventArgs
            {
                Selection = new Rectangle(new Point(10, 10), new Size(10, 10))
            });
        }

        [Test]
        public void ShouldNotPassAnyEventsIfNoRectangleSelected()
        {
            //given
            _rectangleEventsTester = new MouseEventsTester(new RectangleMouseEvents(A.Fake<SelectionProviderThingie>(), _unfilteredMouseEvents));

            //when
            _unfilteredMouseEvents.DoMouseDown(_insideRectangle);
            _unfilteredMouseEvents.DoMouseMove(_insideRectangle);
            _unfilteredMouseEvents.DoMouseUp(_insideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseDownArgs, Is.Empty);
            Assert.That(_rectangleEventsTester.MouseMoveArgs, Is.Empty);
            Assert.That(_rectangleEventsTester.MouseUpArgs, Is.Empty);
        }

        [Test]
        public void ShouldPassMouseEventsInsideRectangleUnchanged()
        {
            //when
            _unfilteredMouseEvents.DoMouseDown(_insideRectangle);
            _unfilteredMouseEvents.DoMouseMove(_insideRectangle);
            _unfilteredMouseEvents.DoMouseUp(_insideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseDownArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_rectangleEventsTester.MouseMoveArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_rectangleEventsTester.MouseUpArgs.Single(), Is.EqualTo(_insideRectangle));
        }

        [Test]
        public void ShouldNotPassMouseMoveDownEventsOutsideRectangle()
        {
            //when
            _unfilteredMouseEvents.DoMouseDown(_outsideRectangle);
            _unfilteredMouseEvents.DoMouseMove(_outsideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseDownArgs, Is.Empty);
            Assert.That(_rectangleEventsTester.MouseMoveArgs, Is.Empty);
        }

        [Test]
        public void ShouldNotPassMouseUpEventsOutsideRectangleIfThereWasNoMoveInside()
        {
            //when
            _unfilteredMouseEvents.DoMouseUp(_outsideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseUpArgs, Is.Empty);
        }

        [Test]
        public void ShouldPassMouseUpEventsOutsideRectangleAfterDownAndMoveInsideWithLastMoveArgs()
        {
            //when
            _unfilteredMouseEvents.DoMouseDown(_insideRectangle);
            _unfilteredMouseEvents.DoMouseMove(_insideRectangle);
            _unfilteredMouseEvents.DoMouseUp(_outsideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseMoveArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_rectangleEventsTester.MouseUpArgs.Single(), Is.EqualTo(_insideRectangle));
        }

        [Test]
        public void ShouldNotPassMouseUpsAfterMovingInsideTheRectangleAndClickingOutside()
        {
            //when
            _unfilteredMouseEvents.DoMouseMove(_insideRectangle);
            _unfilteredMouseEvents.DoMouseDown(_outsideRectangle);
            _unfilteredMouseEvents.DoMouseUp(_outsideRectangle);

            //then
            Assert.That(_rectangleEventsTester.MouseMoveArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_rectangleEventsTester.MouseUpArgs, Is.Empty);
        }
    }
}