using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListBoxItem newItem = new ListBoxItem();
            newItem.Content = "Hello mir";

            newItem.Style = (Style)FindResource("pendingStyle");

            pending.Items.Add(newItem);
        }

        private void changeAvatarBtn_Click(object sender, RoutedEventArgs e)
        {
            avatar.Source = new BitmapImage(new Uri("D:\\itstep\\wpf\\TaskManager\\TaskManager\\AppPictures\\Style\\avatar2.png"));
        }
    }
}
