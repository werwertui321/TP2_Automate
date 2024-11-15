using System.Windows;

namespace Automate.Interfaces
{
    public interface INavigationUtils
    {
        void OpenNewView<T>() where T : Window, new();

        void CloseCurrentView(Window window);
    }

}
