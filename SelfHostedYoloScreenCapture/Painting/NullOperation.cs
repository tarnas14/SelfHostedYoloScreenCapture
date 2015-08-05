namespace SelfHostedYoloScreenCapture.Painting
{
    internal class NullOperation : Operation
    {
        public void Do()
        {
            //I refuse to do anything
        }

        public void Undo()
        {
            //I refuse to do anything
        }
    }
}