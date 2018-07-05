using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            dynamic sndr;
            if (sender is TextBox)
            {
                sndr = (TextBox)sender;
                if (sndr.Text != String.Empty)
                    return;
            }
            else
            {
                sndr = (PasswordBox)sender;
                if (sndr.Password != String.Empty)
                    return;
            }
            sndr.Padding = new Thickness(1, 10, 1, 0);
            var anim = new DoubleAnimation()
            {
                From = sndr.Height,
                To = 40.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(HeightProperty, anim);
            var label = (Label)sndr.Tag;
            var marginValue = (Thickness) label.GetAnimationBaseValue(MarginProperty);
            var a = new ThicknessAnimation()
            {
                From = marginValue,
                To = new Thickness(marginValue.Left, marginValue.Top - 25, marginValue.Right, marginValue.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            label.BeginAnimation(MarginProperty, a);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            dynamic sndr;
            if (sender is TextBox)
            {
                sndr = (TextBox)sender;
                if (sndr.Text != String.Empty)
                    return;
            }
            else
            {
                sndr = (PasswordBox)sender;
                if (sndr.Password != String.Empty)
                    return;
            }
            sndr.Padding = new Thickness(1, 5, 1, 0);
            var anim = new DoubleAnimation()
            {
                From = sndr.Height,
                To = 30.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            sndr.BeginAnimation(HeightProperty, anim);
            var label = (Label)sndr.Tag;
            var marginValue = (Thickness) label.GetAnimationBaseValue(MarginProperty);
            var a = new ThicknessAnimation
            {
                From = new Thickness(marginValue.Left, marginValue.Top - 25, marginValue.Right, marginValue.Bottom),
                To = marginValue,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            label.BeginAnimation(MarginProperty, a);
        }

        private void label_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sndr = (Label)sender;
            if (sndr.Tag is TextBox)
                ((TextBox)sndr.Tag).Focus();
            else
                ((PasswordBox)sndr.Tag).Focus();
        }
    }
}
