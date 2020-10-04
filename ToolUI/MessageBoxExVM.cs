using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WpfEssentials.Win32;

namespace WHampson.ToolUI
{
    /// <summary>
    /// Extended Message Box view model.
    /// </summary>
    public class MessageBoxExVM : DialogVM
    {
        private const string OKText = "OK";
        private const string CancelText = "Cancel";
        private const string YesText = "Yes";
        private const string NoText = "No";

        private IntPtr m_iconPtr;
        private MessageBoxButton m_buttons;
        private MessageBoxImage m_image;
        private MessageBoxResult m_result;
        private BitmapSource m_imageSource;
        private bool m_isIconVisible;
        private ButtonInfo m_button1Config;
        private ButtonInfo m_button2Config;
        private ButtonInfo m_button3Config;
        private string m_text;

        public MessageBoxButton Buttons
        {
            get { return m_buttons; }
            set
            {
                m_buttons = value;
                OnPropertyChanged();
                SetButtonConfig(value);
            }
        }

        public MessageBoxImage Icon
        {
            get { return m_image; }
            set
            {
                m_image = value;
                OnPropertyChanged();
                SetIcon(value);
            }
        }

        public MessageBoxResult Result
        {
            get { return m_result; }
            set { m_result = value; OnPropertyChanged(); }
        }

        public BitmapSource IconSource
        {
            get { return m_imageSource; }
            set { m_imageSource = value; OnPropertyChanged(); }
        }

        public bool IsIconVisible
        {
            get { return m_isIconVisible; }
            set { m_isIconVisible = value; OnPropertyChanged(); }
        }

        public ButtonInfo Button1Info
        {
            get { return m_button1Config; }
            set { m_button1Config = value; OnPropertyChanged(); }
        }

        public ButtonInfo Button2Info
        {
            get { return m_button2Config; }
            set { m_button2Config = value; OnPropertyChanged(); }
        }

        public ButtonInfo Button3Info
        {
            get { return m_button3Config; }
            set { m_button3Config = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get { return m_text; }
            set { m_text = value; OnPropertyChanged(); }
        }

        public MessageBoxExVM()
        {
            Button1Info = new ButtonInfo();
            Button2Info = new ButtonInfo();
            Button3Info = new ButtonInfo();
        }

        public override void Shutdown()
        {
            base.Shutdown();
            DestroyIcon(m_iconPtr);

            if (Result == MessageBoxResult.None)
            {
                Result = Buttons switch
                {
                    MessageBoxButton.OK => MessageBoxResult.OK,
                    MessageBoxButton.YesNo => MessageBoxResult.No,
                    _ => MessageBoxResult.Cancel
                };
            }
        }

        public override void Load()
        {
            base.Load();
            switch (Icon)
            {
                // TODO: configurable thru request event?
                case MessageBoxImage.Information:
                case MessageBoxImage.Question:
                case MessageBoxImage.Warning:
                    SystemSounds.Beep.Play();
                    break;
                case MessageBoxImage.Error:
                    SystemSounds.Hand.Play();
                    break;
            }
        }

        private void SetButtonConfig(MessageBoxButton buttons)
        {
            var buttonConfig = ButtonConfigMap[buttons];
            Button1Info = buttonConfig.Item1;
            Button2Info = buttonConfig.Item2;
            Button3Info = buttonConfig.Item3;
        }

        private void SetIcon(MessageBoxImage image)
        {
            SHSTOCKICONID iconId = image switch
            {
                MessageBoxImage.Error => SHSTOCKICONID.SIID_ERROR,
                MessageBoxImage.Question => SHSTOCKICONID.SIID_HELP,
                MessageBoxImage.Warning => SHSTOCKICONID.SIID_WARNING,
                MessageBoxImage.Information => SHSTOCKICONID.SIID_INFO,
                _ => 0
            };

            if (iconId != 0)
            {
                SHSTOCKICONINFO sii = new SHSTOCKICONINFO { cbSize = (uint) Marshal.SizeOf(typeof(SHSTOCKICONINFO)) };
                Marshal.ThrowExceptionForHR(SHGetStockIconInfo(iconId, SHGSI.SHGSI_ICON, ref sii));
                m_iconPtr = sii.hIcon;
                IconSource = LoadBitmap(System.Drawing.Icon.FromHandle(m_iconPtr).ToBitmap());
                IsIconVisible = true;
            }
            else
            {
                m_iconPtr = IntPtr.Zero;
                IconSource = null;
                IsIconVisible = false;
            }
        }

        private void ButtonExec(ButtonInfo info)
        {
            if (info.IsDefault)
            {
                Result = info.Text switch
                {
                    OKText => MessageBoxResult.OK,
                    YesText => MessageBoxResult.Yes,
                    _ => MessageBoxResult.None
                };
                Close(true);
                return;
            }

            Result = info.Text switch
            {
                CancelText => MessageBoxResult.Cancel,
                NoText => MessageBoxResult.No,
                _ => MessageBoxResult.None
            };
            Close(false);
        }

        private static readonly Dictionary<MessageBoxButton, (ButtonInfo, ButtonInfo, ButtonInfo)> ButtonConfigMap =
            new Dictionary<MessageBoxButton, (ButtonInfo, ButtonInfo, ButtonInfo)>()
        {
            { MessageBoxButton.OK,          (new ButtonInfo("", false, false),      new ButtonInfo("", false, false),       new ButtonInfo(OKText, true, true)) },
            { MessageBoxButton.OKCancel,    (new ButtonInfo("", false, false),      new ButtonInfo(OKText, true, false),    new ButtonInfo(CancelText, false, true)) },
            { MessageBoxButton.YesNo,       (new ButtonInfo("", false, false),      new ButtonInfo(YesText, true, false),   new ButtonInfo(NoText, false, true)) },
            { MessageBoxButton.YesNoCancel, (new ButtonInfo(YesText, true, false),  new ButtonInfo(NoText, false, false),   new ButtonInfo(CancelText, false, true)) },
        };

        #region Commands
        public ICommand Button1Command => new RelayCommand
        (
            () => ButtonExec(Button1Info),
            () => Button1Info.IsVisible
        );

        public ICommand Button2Command => new RelayCommand
        (
            () => ButtonExec(Button2Info),
            () => Button2Info.IsVisible
        );

        public ICommand Button3Command => new RelayCommand
        (
            () => ButtonExec(Button3Info),
            () => Button3Info.IsVisible
        );

        public ICommand SpaceBarCommand => new RelayCommand
        (
            () =>
            {
                if (Button1Info.IsDefault)
                {
                    ButtonExec(Button1Info);
                }
                else if (Button2Info.IsDefault)
                {
                    ButtonExec(Button2Info);
                }
                else if (Button3Info.IsDefault)
                {
                    ButtonExec(Button3Info);
                }
            }
        );
        #endregion

        #region ButtonInfo
        public class ButtonInfo
        {
            public string Text { get; set; }
            public bool IsVisible { get; set; }
            public bool IsDefault { get; set; }
            public bool IsCancel { get; set; }

            public ButtonInfo()
            { }

            public ButtonInfo(string text, bool isDefault, bool isCancel)
            {
                Text = text;
                IsVisible = !string.IsNullOrEmpty(text);
                IsDefault = isDefault;
                IsCancel = isCancel;
            }
        }
        #endregion

        #region Win32 Interop
        internal enum SHSTOCKICONID : uint
        {
            SIID_HELP = 23,
            SIID_WARNING = 78,
            SIID_INFO = 79,
            SIID_ERROR = 80,
        }

        [Flags]
        internal enum SHGSI : uint
        {
            SHGSI_ICON = 0x000000100,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SHSTOCKICONINFO
        {
            public uint cbSize;
            public IntPtr hIcon;
            public int iSysIconIndex;
            public int iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260 /*MAX_PATH*/)]
            public string szPath;
        }

        internal static BitmapSource LoadBitmap(Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        [DllImport("Shell32.dll", SetLastError = false)]
        static extern int SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("gdi32.dll")]
        static extern int DeleteObject(IntPtr hObject);
        #endregion
    }
}
