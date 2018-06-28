using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoBesedkaApp
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FIOTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var sndr = (TextBox) sender;
            
            Label label = new Label
            {
                Content = sndr.Text,
                FontSize = sndr.FontSize,
                Margin = new Thickness(sndr.Margin.Left - 2, sndr.Margin.Top - 4, sndr.Margin.Right, sndr.Margin.Bottom),
                Height = sndr.Height,
                Width = sndr.Width,
                HorizontalAlignment = sndr.HorizontalAlignment,
                VerticalAlignment = sndr.VerticalAlignment
            };
            sndr.Text = String.Empty;
            sndr.Tag = label;
            RegGrid.Children.Add(label);

            var a = new ThicknessAnimation()
            {
                From = label.Margin,
                To = new Thickness(label.Margin.Left, label.Margin.Top - 25, label.Margin.Right, label.Margin.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            label.BeginAnimation(MarginProperty, a);
        }

        private void FIOTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var sndr = (TextBox) sender;
            var label = (Label)sndr.Tag;

            var a = new ThicknessAnimation
            {
                From = label.Margin,
                To = new Thickness(label.Margin.Left, label.Margin.Top + 25, label.Margin.Right, label.Margin.Bottom),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            a.Completed += anim_Completed;
            label.BeginAnimation(MarginProperty, a);
            //
        }

        private void anim_Completed(object sender, EventArgs e)
        {

            //RegGrid.Children.Remove((Label)sndr.Tag);
        }
    }
}
