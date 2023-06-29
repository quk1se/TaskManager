using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
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
        public string newImgPath;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void changeAvatarBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Выберите файл"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string newImgPath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                try
                {
                    BitmapDecoder decoder = BitmapDecoder.Create(new Uri(newImgPath), BitmapCreateOptions.None, BitmapCacheOption.OnDemand);
                    avatar.Source = new BitmapImage(new Uri(newImgPath));

                    avatar.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Ошибка, некорректный формат файла изображения!");
                    return;
                }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("Ошибка, выберите изображение!");
                    return;
                }
            }
        }
        private void AddTask(ListBox listName, string taskName, string pictureName)
        {
            Style listTxtStyle = (Style)FindResource("listTxt");

            ListBoxItem newItem = new ListBoxItem();
            newItem.Content = taskName;

            DataTemplate dataTemplate = new DataTemplate();
            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));
            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));

            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            imageFactory.SetValue(Image.SourceProperty, new BitmapImage(new Uri($"D:\\itstep\\wpf\\TaskManager\\TaskManager\\AppPictures\\Style\\{pictureName}.ico")));
            imageFactory.SetValue(WidthProperty, 32.0);
            imageFactory.SetValue(HeightProperty, 32.0);
            imageFactory.SetValue(MarginProperty, new Thickness(0, 10, 0, 0));
            imageFactory.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);

            textBlockFactory.SetValue(TextBlock.TextProperty, new Binding());
            textBlockFactory.SetValue(StyleProperty, listTxtStyle);
            textBlockFactory.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            textBlockFactory.SetValue(MarginProperty, new Thickness(10,15,10,8));

            stackPanelFactory.AppendChild(imageFactory);
            stackPanelFactory.AppendChild(textBlockFactory);

            dataTemplate.VisualTree = stackPanelFactory;

            newItem.ContentTemplate = dataTemplate;

            listName.Items.Add(newItem);
        }

        private void addTask_Click(object sender, RoutedEventArgs e)
        {
            WindowNewTask window = new WindowNewTask();
            window.ShowDialog();
            if (window.is_click)
                AddTask(pending,window.taskNameTxt,"pending");
        }

        public void RemoveTo(ListBox listTo, ListBox listFrom, string pictureName)
        {
            try
            {
                ListBoxItem selectedItem = listTo.SelectedItem as ListBoxItem;
                if (selectedItem != null)
                {
                    AddTask(listFrom, selectedItem.Content.ToString(), pictureName);
                    listTo.Items.Remove(listTo.SelectedItem);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Select a task!");
            }
        }

        private void toProgressBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(pending, in_progress, "progress");
        }

        private void toPendingBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(in_progress, pending, "pending");
        }

        private void toCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(in_progress, completed, "completed");
        }

        private void toProgressFromCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(completed, in_progress, "progress");
        }
    }
}
