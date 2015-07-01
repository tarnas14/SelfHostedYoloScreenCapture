namespace SelfHostedYoloScreenCapture
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Properties;

    public class TrayIcon : IDisposable
    {
        private readonly NotifyIcon _icon;

        public event EventHandler Exit; 

        public TrayIcon()
        {
            _icon = new NotifyIcon();
            SetupIcon();
        }

        private void SetupIcon()
        {
            _icon.Icon = Icon.FromHandle(Resources.PlaceholderIcon.Handle);
            _icon.Text = "Yaay";
            _icon.Visible = true;

            SetupRightClickMenu(_icon);
        }

        private void SetupRightClickMenu(NotifyIcon icon)
        {
            var contextMenu = new ContextMenu();
            var exit = contextMenu.MenuItems.Add("Exit");
            icon.ContextMenu = contextMenu;

            exit.Click += FireExitEvent;
        }

        private void FireExitEvent(object sender, EventArgs e)
        {
            if (Exit != null)
            {
                Exit(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            _icon.Dispose();
        }
    }
}