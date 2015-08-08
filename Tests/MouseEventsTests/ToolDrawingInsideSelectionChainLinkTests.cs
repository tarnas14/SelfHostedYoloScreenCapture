namespace Tests.MouseEventsTests
{
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using FakeItEasy;
    using NUnit.Framework;
    using SelfHostedYoloScreenCapture;
    using SelfHostedYoloScreenCapture.Painting;
    using SelfHostedYoloScreenCapture.SelectingRectangle;

    class ToolDrawingInsideSelectionChainLinkTests
    {
        private MouseEventsTester _toolEventsTester;
        private MouseEventArgs _insideRectangle;
        private MouseEventArgs _outsideRectangle;
        private ToolDrawingInsideSelectionChainLink _toolDrawingInsideSelectionChainLink;
        private object _sender;
        private SelectionProviderThingie _selectionProvider;
        private ResponsibilityChainLink _nextLink;

        [SetUp]
        public void Setup()
        {
            _sender = new object();
            _insideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 11, 11, 0);
            _outsideRectangle = new MouseEventArgs(MouseButtons.Left, 1, 24, 13, 0);

            _selectionProvider = A.Fake<SelectionProviderThingie>();
            _nextLink = A.Fake<ResponsibilityChainLink>();
            _toolDrawingInsideSelectionChainLink = new ToolDrawingInsideSelectionChainLink(_selectionProvider, _nextLink);
            _toolEventsTester = new MouseEventsTester(_toolDrawingInsideSelectionChainLink);
        }

        private void SelectSelection()
        {
            _selectionProvider.RectangleSelected += Raise.With(this, new RectangleSelectedEventArgs
            {
                Selection = new Rectangle(new Point(10, 10), new Size(10, 10))
            });
        }

        [Test]
        public void ShouldCallNextLinkIfSelectionHasNotBeenSelected()
        {
            //when
            _toolDrawingInsideSelectionChainLink.DoMouseDown(_sender, _insideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseMove(_sender, _insideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseUp(_sender, _insideRectangle);

            //then
            Assert.That(_toolEventsTester.MouseDownArgs, Is.Empty);
            Assert.That(_toolEventsTester.MouseMoveArgs, Is.Empty);
            Assert.That(_toolEventsTester.MouseUpArgs, Is.Empty);

            A.CallTo(() => _nextLink.DoMouseDown(_sender, _insideRectangle)).MustHaveHappened();
            A.CallTo(() => _nextLink.DoMouseMove(_sender, _insideRectangle)).MustHaveHappened();
            A.CallTo(() => _nextLink.DoMouseUp(_sender, _insideRectangle)).MustHaveHappened();
        }

        [Test]
        public void ShouldInvokeEventsFromInsideTheRectangleUnchanged()
        {
            //given
            SelectSelection();

            //when
            _toolDrawingInsideSelectionChainLink.DoMouseDown(_sender, _insideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseMove(_sender, _insideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseUp(_sender, _insideRectangle);

            //then
            Assert.That(_toolEventsTester.MouseDownArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_toolEventsTester.MouseMoveArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_toolEventsTester.MouseUpArgs.Single(), Is.EqualTo(_insideRectangle));

            A.CallTo(() => _nextLink.DoMouseDown(_sender, A<MouseEventArgs>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _nextLink.DoMouseMove(_sender, A<MouseEventArgs>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _nextLink.DoMouseUp(_sender, A<MouseEventArgs>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void ShouldCallNextLinkForMouseMoveDownEventsOutsideRectangle()
        {
            //given
            SelectSelection();

            //when
            _toolDrawingInsideSelectionChainLink.DoMouseDown(_sender, _outsideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseMove(_sender, _outsideRectangle);

            //then
            Assert.That(_toolEventsTester.MouseDownArgs, Is.Empty);
            Assert.That(_toolEventsTester.MouseMoveArgs, Is.Empty);

            A.CallTo(() => _nextLink.DoMouseDown(_sender, _outsideRectangle)).MustHaveHappened();
            A.CallTo(() => _nextLink.DoMouseMove(_sender, _outsideRectangle)).MustHaveHappened();
        }

        [Test]
        public void ShouldCallNextLinkForMouseUpEventsOutsideRectangleIfThereWasNoMoveInside()
        {
            //given
            SelectSelection();

            //when
            _toolDrawingInsideSelectionChainLink.DoMouseUp(_sender, _outsideRectangle);

            //then
            Assert.That(_toolEventsTester.MouseUpArgs, Is.Empty);

            A.CallTo(() => _nextLink.DoMouseUp(_sender, _outsideRectangle)).MustHaveHappened();
        }

        [Test]
        public void ShouldInvokeMouseUpEventsOutsideRectangleAfterDownAndMoveInsideWithLastMoveArgs()
        {
            //given
            SelectSelection();

            //when
            _toolDrawingInsideSelectionChainLink.DoMouseDown(_sender, _insideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseMove(_sender, _insideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseUp(_sender, _outsideRectangle);

            //then
            Assert.That(_toolEventsTester.MouseDownArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_toolEventsTester.MouseMoveArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_toolEventsTester.MouseUpArgs.Single(), Is.EqualTo(_insideRectangle));

            A.CallTo(() => _nextLink.DoMouseDown(_sender, A<MouseEventArgs>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _nextLink.DoMouseMove(_sender, A<MouseEventArgs>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _nextLink.DoMouseUp(_sender, A<MouseEventArgs>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void ShouldNotInvokeMouseUpsAfterMovingInsideTheRectangleAndClickingOutside()
        {
            //given
            SelectSelection();

            //when
            _toolDrawingInsideSelectionChainLink.DoMouseMove(_sender, _insideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseDown(_sender, _outsideRectangle);
            _toolDrawingInsideSelectionChainLink.DoMouseUp(_sender, _outsideRectangle);

            //then
            Assert.That(_toolEventsTester.MouseDownArgs, Is.Empty);
            Assert.That(_toolEventsTester.MouseMoveArgs.Single(), Is.EqualTo(_insideRectangle));
            Assert.That(_toolEventsTester.MouseUpArgs, Is.Empty);

            A.CallTo(() => _nextLink.DoMouseDown(_sender, _outsideRectangle)).MustHaveHappened();
            A.CallTo(() => _nextLink.DoMouseUp(_sender, _outsideRectangle)).MustHaveHappened();
        }
    }

    public interface ResponsibilityChainLink
    {
        void DoMouseDown(object sender, MouseEventArgs args);
        void DoMouseMove(object sender, MouseEventArgs args);
        void DoMouseUp(object sender, MouseEventArgs args);
    }

    internal class ToolDrawingInsideSelectionChainLink : MouseEvents
    {
        private readonly ResponsibilityChainLink _nextLink;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;
        private Rectangle _rectangle;
        private bool _isMouseDownInsideRectangle;
        private MouseEventArgs _lastMoveArgs;

        public ToolDrawingInsideSelectionChainLink(SelectionProviderThingie selectionProviderThingie, ResponsibilityChainLink nextLink)
        {
            _nextLink = nextLink;
            selectionProviderThingie.RectangleSelected += (sender, args) => { _rectangle = args.Selection; };
        }

        public void DoMouseDown(object sender, MouseEventArgs args)
        {
            if (!InsideRectangle(args))
            {
                _nextLink.DoMouseDown(sender, args);
                return;
            }

            if (MouseDown == null)
            {
                return;
            }

            MouseDown(sender, args);
            _isMouseDownInsideRectangle = true;
        }

        public void DoMouseMove(object sender, MouseEventArgs args)
        {
            if (!InsideRectangle(args))
            {
                _nextLink.DoMouseMove(sender, args);
                return;
            }

            if (MouseMove == null)
            {
                return;
            }

            _lastMoveArgs = args;
            MouseMove(sender, args);
        }

        public void DoMouseUp(object sender, MouseEventArgs args)
        {
            if (MouseUp == null)
            {
                return;
            }

            if (InsideRectangle(args))
            {
                MouseUp(sender, args);
                return;
            }

            if (_isMouseDownInsideRectangle && _lastMoveArgs != null && !InsideRectangle(args))
            {
                MouseUp(sender, _lastMoveArgs);
                _isMouseDownInsideRectangle = false;
                _lastMoveArgs = null;
                return;
            }

            _nextLink.DoMouseUp(sender, args);
        }

        private bool InsideRectangle(MouseEventArgs mouseEventArgs)
        {
            var xInsideRectangle = mouseEventArgs.X >= _rectangle.Left && mouseEventArgs.X <= _rectangle.Right;
            var yInsideRectangle = mouseEventArgs.Y >= _rectangle.Top && mouseEventArgs.Y <= _rectangle.Bottom;

            return xInsideRectangle && yInsideRectangle;
        }
    }
}