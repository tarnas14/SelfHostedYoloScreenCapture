namespace Tests
{
    using FakeItEasy;
    using NUnit.Framework;
    using SelfHostedYoloScreenCapture;

    class OpertionQueueTests
    {
        private OperationQueue _queue;

        [SetUp]
        public void Setup()
        {
            _queue = new OperationQueue();
        }

        [Test]
        public void ShouldExecuteEachAddedOperation()
        {
            //given
            var operation = A.Fake<Operation>();
            var anotherOperation = A.Fake<Operation>();

            //when
            _queue.Execute(operation);
            _queue.Execute(anotherOperation);

            //then
            A.CallTo(() => operation.Do()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => anotherOperation.Do()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ShouldUndoOperationWhenBackingInQueue()
        {
            //given
            var operation = A.Fake<Operation>();
            _queue.Execute(operation);

            //when
            _queue.Back();

            //then
            A.CallTo(() => operation.Undo()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ShouldUndoAndRedoOperation()
        {
            //given
            var firstOperation = A.Fake<Operation>();

            _queue.Execute(firstOperation);

            _queue.Back();

            //when
            _queue.Forward();

            //then
            A.CallTo(() => firstOperation.Do()).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => firstOperation.Undo()).MustHaveHappened(Repeated.Exactly.Once);
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
    }
}