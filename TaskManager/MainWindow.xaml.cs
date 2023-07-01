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

        private string newImgPath; // Image for task state

        public string NewImgPath
        {
            get { return newImgPath; }
            set { newImgPath = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        // Button to change your profile picture 
        private void changeAvatarBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Choose file"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string NewImgPath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                try
                {
                    BitmapDecoder decoder = BitmapDecoder.Create(new Uri(NewImgPath), BitmapCreateOptions.None, BitmapCacheOption.OnDemand);
                    avatar.Source = new BitmapImage(new Uri(NewImgPath));

                    avatar.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Error, incorrect image file format!");
                    return;
                }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("Error, pick the image!");
                    return;
                }
            }
        }
        // Method to add new task in pending list
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

        // Button for add new task in list
        private void addTask_Click(object sender, RoutedEventArgs e)
        {
            WindowNewTask window = new WindowNewTask();
            window.ShowDialog();
            if (window.is_click)
                AddTask(pending,window.taskNameTxt,"pending");
        }

        // Method to removing select task to a different other task state
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
        // Button for removing select task from pending to progress states
        private void toProgressBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(pending, in_progress, "progress");
        }
        // Button for removing select task from progress to pending states
        private void toPendingBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(in_progress, pending, "pending");
        }
        // Button for removing select task from progress to completed states
        private void toCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(in_progress, completed, "completed");
        }
        // Button for removing select task from completed to progress states
        private void toProgressFromCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(completed, in_progress, "progress");
        }
    }
}
