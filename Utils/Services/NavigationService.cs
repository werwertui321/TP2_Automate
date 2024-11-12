using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Automate.Utils.Services
{
    public class NavigationService
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
