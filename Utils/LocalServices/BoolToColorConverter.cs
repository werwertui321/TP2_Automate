using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Automate.Utils
{

    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush defaultColor = Brushes.Transparent;

            if (value is bool boolValue)
            {
                SolidColorBrush? red = new BrushConverter().ConvertFrom("#c50500") as SolidColorBrush;

                return boolValue ? red! : defaultColor;
            }
            return defaultColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
