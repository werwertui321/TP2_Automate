using Automate.Utils.LocalServices;
using Automate.Views;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    class HomeViewModel
    {
        private readonly NavigationUtils _navigationService;
        public ICommand GoToCalendarCommand { get; }

        private Window _window;
        public HomeViewModel(Window openedWindow, NavigationUtils navigationService)
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
