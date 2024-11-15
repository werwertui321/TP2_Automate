using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Automate.Interfaces
{
    public interface INavigationUtils
    {
        void OpenNewView<T>() where T : Window, new();

        void CloseCurrentView(Window window);
    }

}
