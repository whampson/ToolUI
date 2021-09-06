using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Interaction logic for MessageBoxEx.xaml
    /// </summary>
    public partial class MessageBoxEx : WindowBase
    {
        const int GWL_EXSTYLE = -20;
        const int WS_EX_DLGMODALFRAME = 0x0001;

        public MessageBoxEx()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            // Remove app icon from top left corner
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            uint extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

            base.OnSourceInitialized(e);
        }

        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    }
}
