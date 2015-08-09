namespace SelfHostedYoloScreenCapture.Drawing
{
    using System.Drawing;
    using SelectingRectangle;

    public interface Tool
    {
        Operation GetUndoableOperation(Image canvas, Point start, Point end);
        void DrawOn(SelectionCanvas canvas, Point start, Point end);
    }
}