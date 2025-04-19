using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AIHomeProject.Converters
{
    // Converters/CountToVisibilityConverter.cs
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count == 0;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
