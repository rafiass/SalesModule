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
            else if (targetType.GenericTypeArguments.Length > 0 &&
                targetType.GenericTypeArguments[0].FullName == "System.Boolean" ||
                targetType.FullName == "System.Boolean")
                return value.Equals(parameter);
            else throw new NotImplementedException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return parameter;
            else return Binding.DoNothing;
        }
    }
    internal class NullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? parameter;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //internal class BoolToVisibilityConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //    }
    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //    }
    //}

    internal class NoOpConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
