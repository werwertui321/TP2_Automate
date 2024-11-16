using Automate.ViewModels;
using Automate.Utils;
using System.Windows;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        public CalendarWindow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel(this, Env.calendarService, Env.authenticatedUser!.IsAdmin);
        }
    }
}
