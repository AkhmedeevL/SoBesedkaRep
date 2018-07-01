using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SoBesedkaDB.Views;

namespace SoBesedkaApp
{
    public class CreatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int) value;
            UserViewModel user;
            try
            {
                var response = APIClient.GetRequest("api/User/Get/" + id);
                if (response.Result.IsSuccessStatusCode)
                {
                    user = APIClient.GetElement<UserViewModel>(response);
                    if (user == null)
                        return "";
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            return user.UserFIO;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}