using Automate.ViewModels;
using Automate.Utils;
using System.Windows;
using System.Windows.Controls;
using Automate.Utils.LocalServices;

namespace Automate
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel(this, Env.userService, new NavigationUtils());
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox? passwordBox = sender as PasswordBox;
            if (DataContext is CalendarViewModel viewModel)
            {
                viewModel.Password = passwordBox.Password;
            }
        }
    }
}
