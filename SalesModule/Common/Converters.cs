using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SalesModule
{
    internal class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            if (targetType == typeof(Visibility))
                return value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
            else if (targetType == typeof(bool))
                return value.Equals(parameter);
            else throw new NotImplementedException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    //internal class BoolToVisibilityConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null || parameter == null) return Visibility.Collapsed;
    //        if (targetType == typeof(Visibility))
    //            return value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
    //        else if (targetType == typeof(bool))
    //            return value.Equals(parameter);
    //        else throw new NotImplementedException();
    //    }
    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return null;
    //    }
    //}
}
