namespace SelfHostedYoloScreenCapture.Drawing
{
    using System.Drawing;
    using SelectingRectangle;

    internal class NullTool : Tool
    {
        public Operation GetUndoableOperation(Image canvas, Point start, Point end)
        {
            //I refuse to do anything
            return new NullOperation();
        }

        public void DrawOn(SelectionCanvas canvas, Point start, Point end)
        {
            //I refuse to do anything
        }
    }
}