namespace SelfHostedYoloScreenCapture.Configuration
{
    using System.Windows.Forms;

    internal class HotkeyConfiguration
    {
        public bool Alt { get; set; }
        public bool Ctrl { get; set; }
        public Keys KeyCode { get; set; }
        public bool Shift { get; set; }
        public bool WindowsKey { get; set; }
    }
}