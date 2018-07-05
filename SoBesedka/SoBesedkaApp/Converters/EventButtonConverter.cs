using System;
using System.Windows.Data;
using System.Windows.Media;
using SoBesedkaDB.Views;

namespace SoBesedkaApp
{
    public class EventButtonConverter : IMultiValueConverter  
    {  
 
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var a = ((MeetingViewModel)values[0]).CreatorId.ToString();  
            var b = ((UserViewModel)values[1]).Id.ToString();  
            
            if (a == b)  
                return Brushes.CornflowerBlue;  
            else  
                return Brushes.LightGray;  
        }  
 
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)  
        {  
            throw new NotImplementedException();  
        }  
 
    }
}
