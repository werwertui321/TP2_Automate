using System.Windows;
using Automate.Interfaces;

namespace Automate.Utils.LocalServices
{
    public class NavigationUtils : INavigationUtils
    {
        public void OpenNewView<T>() where T : Window, new()
        {
            var window = new T();
            window.Show();
        }

        public void CloseCurrentView(Window window)
        {
            window.Close();
        }
    }
}
