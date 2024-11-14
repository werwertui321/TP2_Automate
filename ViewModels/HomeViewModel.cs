using Automate.Utils.LocalServices;
using Automate.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    class HomeViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand GoToCalendarCommand { get; }

        private Window _window;
        public HomeViewModel(Window openedWindow, NavigationService navigationService)
        {
            GoToCalendarCommand = new RelayCommand(GotoCalendarView);
            _navigationService = navigationService;
            _window = openedWindow;
        }

        public void GotoCalendarView()
        {
            _navigationService.OpenNewView<CalendarWindow>();
            _navigationService.CloseCurrentView(_window);
        }
    }
}
