using System.Windows.Input;
using WpfEssentials.Win32;

namespace ToolUI.Test
{
    public class StatusTextVM : TabPageVM
    {
        private string m_status;
        private string m_timedStatus;
        private double m_statusDuration;

        public string Status
        {
            get { return m_status; }
            set { m_status = value; OnPropertyChanged(); }
        }

        public string TimedStatus
        {
            get { return m_timedStatus; }
            set { m_timedStatus = value; OnPropertyChanged(); }
        }

        public double StatusDuration
        {
            get { return m_statusDuration; }
            set { m_statusDuration = value; OnPropertyChanged(); }
        }

        public StatusTextVM()
        {
            Status = "Ready.";
            TimedStatus = "She sells sea shells by the sea shore.";
        }

        public override void Init()
        {
            base.Init();

            StatusDuration = 5;
            MainWindow.SetStatusText(Status);
        }

        public ICommand SetStatusCommand => new RelayCommand
        (
            () => MainWindow.SetStatusText(Status)
        );

        public ICommand SetTimedStatusCommand => new RelayCommand
        (
            () => MainWindow.SetTimedStatusText(TimedStatus, StatusDuration)
        );
    }
}
