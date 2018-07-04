using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoBesedkaApp
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var day = (DateTime)value;
            return day.ToString("dd.MM") + "\n" + CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day.DayOfWeek);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}