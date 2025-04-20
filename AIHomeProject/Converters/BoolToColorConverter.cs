using System.Globalization;

namespace AIHomeProject.Converters
{
    // BoolToColorConverter.cs
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Color.FromHex("#512BD4") : Color.FromHex("#B0B0B0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
