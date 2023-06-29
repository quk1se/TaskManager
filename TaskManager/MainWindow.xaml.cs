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
            AddNewElement(pending);
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
        private void AddNewElement(ListBox listName)
        {
            Style pendingStyle = (Style)FindResource("pendingStyle");
            Style listTxtStyle = (Style)FindResource("listTxt");

            ListBoxItem newItem = new ListBoxItem();
            newItem.Content = "asd";

            DataTemplate dataTemplate = new DataTemplate();
            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));
            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));

            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            imageFactory.SetValue(Image.SourceProperty, new BitmapImage(new Uri("D:\\itstep\\wpf\\TaskManager\\TaskManager\\AppPictures\\Style\\pending.ico")));
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
            newItem.Style = pendingStyle;

            listName.Items.Add(newItem);
        }

    }
}
