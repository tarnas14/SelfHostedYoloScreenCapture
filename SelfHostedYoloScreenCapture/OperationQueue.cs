namespace SelfHostedYoloScreenCapture
{
    using System.Collections.Generic;
    using System.Linq;

    public class OperationQueue
    {
        private readonly IList<Operation> _operations;
        private int _currentIndex;

        private Operation CurrentOperation
        {
            get { return _operations[_currentIndex]; }
        }

        public OperationQueue()
        {
            _operations = new List<Operation>();
            _currentIndex = -1;
        }

        public void Execute(Operation operation)
        {
            operation.Do();
            Add(operation);
        }

        private void Add(Operation operation)
        {
            if (_currentIndex != _operations.Count - 1)
            {
                var operationsToRemove = _operations.Skip(_currentIndex + 1).ToList();

                foreach (var operationToRemove in operationsToRemove)
                {
                    _operations.Remove(operationToRemove);
                }
            }

            _operations.Add(operation);
            _currentIndex++;
        }

        public void Back()
        {
            if (_currentIndex == -1)
            {
                return;
            }

            CurrentOperation.Undo();
            _currentIndex--;
        }

        public void Forward()
        {
            if (_currentIndex == _operations.Count - 1)
            {
                return;
            }

            _currentIndex++;
            CurrentOperation.Do();
        }
    }
}