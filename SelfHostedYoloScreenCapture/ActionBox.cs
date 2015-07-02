using System.Windows.Forms;

namespace SelfHostedYoloScreenCapture
{
    using System;

    public partial class ActionBox : UserControl
    {
        public event EventHandler Upload;

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
        }

        public void DrawCloseTo(object sender, RectangleSelectedEventArgs args)
        {
            Visible = true;
        }
    }
}
