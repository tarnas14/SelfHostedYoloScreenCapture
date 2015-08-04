namespace SelfHostedYoloScreenCapture.SelectingRectangle
{
    using System.Drawing;
    using System.Windows.Forms;

    class CanvasFromPictureBox : SelectionCanvas
    {
        private readonly PictureBox _pictureBox;

        public CanvasFromPictureBox(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;
        }

        public Image Canvas
        {
            get { return _pictureBox.Image; }
        }

        public void Invalidate()
        {
            Invalidate(new Rectangle(new Point(0,0), _pictureBox.Image.Size));
        }

        public void Invalidate(Rectangle rectangle)
        {
            _pictureBox.Invalidate(rectangle);
        }
    }
}
