using System.Windows.Forms;

namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using Painting;
    using SelectingRectangle;

    public partial class ActionBox : UserControl
    {
        public event EventHandler Upload;
        public event KeyEventHandler KeyDownBubble;
        public event EventHandler<ToolSelectedEventArgs> ToolSelected;

        public ActionBox()
        {
            Visible = false;
            InitializeComponent();
            SetupEvents();
        }

        private void SetupEvents()
        {
            _upload.Click += (sender, args) =>
            {
                if (Upload != null)
                {
                    Upload(this, EventArgs.Empty);
                }
            };

            BubbleKeyDown(_upload);

            _testButton.Click += (sender, args) =>
            {
                if (ToolSelected != null)
                {
                    ToolSelected(this,  new ToolSelectedEventArgs
                    {
                        Tool = new NullTool()
                    });
                }
            };
            BubbleKeyDown(_testButton);
        }

        private void BubbleKeyDown(Control bubbledControl)
        {
            bubbledControl.KeyDown += (sender, args) =>
            {
                if (KeyDownBubble != null)
                {
                    KeyDownBubble(sender, args);
                }
            };
        }

        public void DrawCloseTo(object sender, RectangleSelectedEventArgs args)
        {
            var actionBoxWidth = Width;
            var actionBoxHeight = Height;

            var roomAboveSelection = args.Selection.Y;
            var x = args.Selection.Right - actionBoxWidth;

            if (roomAboveSelection >= actionBoxHeight)
            {
                var y = args.Selection.Y - actionBoxHeight;
                Location = new Point(x, y);
                Visible = true;
                return;
            }

            var roomBelowSelection = args.CanvasSize.Height - args.Selection.Bottom;

            if (roomBelowSelection >= actionBoxHeight)
            {
                var y = args.Selection.Bottom;
                Location = new Point(x, y);
                Visible = true;
            }
        }

        public void HideActions(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
