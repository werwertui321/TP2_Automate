using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Automate.Utils.LocalServices
{

    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush defaultColor = Brushes.Transparent;

            if (value is bool boolValue)
            {
                SolidColorBrush? red = new BrushConverter().ConvertFrom("#ff050d") as SolidColorBrush;

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
