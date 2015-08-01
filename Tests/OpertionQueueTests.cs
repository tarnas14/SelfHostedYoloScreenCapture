namespace Tests
{
    using FakeItEasy;
    using NUnit.Framework;
    using SelfHostedYoloScreenCapture;

    class OpertionQueueTests
    {
        private OperationQueue _queue;
        private Operation _operation;

        [SetUp]
        public void Setup()
        {
            _queue = new OperationQueue();
            _operation = A.Fake<Operation>();
        }

        [Test]
        public void ShouldExecuteEachAddedOperation()
        {
            //given
            var anotherOperation = A.Fake<Operation>();

            //when
            _queue.Execute(_operation);
            _queue.Execute(anotherOperation);

            //then
            A.CallTo(() => _operation.Do()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => anotherOperation.Do()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ShouldUndoOperationWhenBackingInQueue()
        {
            //given
            _queue.Execute(_operation);

            //when
            _queue.Back();

            //then
            A.CallTo(() => _operation.Undo()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ShouldUndoAndRedoOperation()
        {
            //given
            _queue.Execute(_operation);

            _queue.Back();

            //when
            _queue.Forward();

            //then
            A.CallTo(() => _operation.Do()).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => _operation.Undo()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ShouldRetainHistoryOfOperations()
        {
            //given
            var firstOperation = A.Fake<Operation>();
            var secondOperation = A.Fake<Operation>();

            //whens -> thens
            _queue.Execute(firstOperation);
            A.CallTo(() => firstOperation.Do()).MustHaveHappened(Repeated.Exactly.Once);

            _queue.Execute(secondOperation);
            A.CallTo(() => secondOperation.Do()).MustHaveHappened(Repeated.Exactly.Once);

            _queue.Back();
            A.CallTo(() => secondOperation.Do()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => secondOperation.Undo()).MustHaveHappened(Repeated.Exactly.Once);

            _queue.Back();
            A.CallTo(() => firstOperation.Do()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => firstOperation.Undo()).MustHaveHappened(Repeated.Exactly.Once);

            _queue.Forward();
            A.CallTo(() => firstOperation.Undo()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => firstOperation.Do()).MustHaveHappened(Repeated.Exactly.Twice);

            _queue.Forward();
            A.CallTo(() => secondOperation.Do()).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => secondOperation.Undo()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void BackingTooMuchShouldBeANoOp()
        {
            //given
            _queue.Execute(_operation);
            _queue.Back();

            //when
            _queue.Back();

            //then
            A.CallTo(() => _operation.Do()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _operation.Undo()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ForwardingTooMuchShouldBeANoOp()
        {
            //given
            _queue.Execute(_operation);

            //when
            _queue.Forward();

            //then
            A.CallTo(() => _operation.Do()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _operation.Undo()).MustHaveHappened(Repeated.Never);
        }
    }
}