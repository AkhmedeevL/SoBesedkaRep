using SoBesedkaDB.Views;
using System.Windows;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для MeetingInfo.xaml
    /// </summary>
    public partial class MeetingInfo : Window
    {
        MeetingViewModel Meeting;
        public MeetingInfo(MeetingViewModel meeting)
        {
            Meeting = meeting;
            DataContext = Meeting;
            InitializeComponent();
        }
    }
}
