using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using System.Windows.Threading;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace TaskManager
{
    public partial class MainWindow : Window
    { 

        private string newImgPath; // Image for task state
        private Dictionary<string,DateTime> taskListPending = new Dictionary<string, DateTime>();
        private Dictionary<string, DateTime> taskListProgress = new Dictionary<string, DateTime>();
        private Dictionary<string, DateTime> taskListComplete = new Dictionary<string, DateTime>();
        private Dictionary<string, DateTime> taskListOverdue = new Dictionary<string, DateTime>();
        private DispatcherTimer timer;

        public string NewImgPath
        {
            get { return newImgPath; }
            set { newImgPath = value; }
        }
        public Dictionary<string, DateTime> TaskListPending
        {
            get { return taskListPending; }
            set { taskListPending = value; }
        }
        public Dictionary<string, DateTime> TaskListProgress
        {
            get { return taskListProgress; }
            set { taskListProgress = value; }
        }
        public Dictionary<string, DateTime> TaskListComplete
        {
            get { return taskListComplete; }
            set { taskListComplete = value; }
        }
        public Dictionary<string, DateTime> TaskListOverdue
        {
            get { return taskListOverdue; }
            set { taskListOverdue = value; }
        }
        public DispatcherTimer Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(deleteComplete, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(deleteOverdue, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(deletePending, BitmapScalingMode.HighQuality);
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(60);
            Timer.Tick += Timer_Tick;
            Timer.Start();
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
            {
                try
                {
                    if (TaskListProgress.ContainsKey(window.taskNameTxt) || TaskListComplete.ContainsKey(window.taskNameTxt) || TaskListOverdue.ContainsKey(window.taskNameTxt))
                    {
                        MessageBox.Show("Enter another task name please!"); return;
                    }
                    TaskListPending.Add(window.taskNameTxt, window.taskDeadline);
                }
                catch (System.ArgumentException)
                {
                    MessageBox.Show("Enter another task name please!");
                    return;
                }
                AddTask(pending, window.taskNameTxt, "pending");
            }

        }

        // Method to removing select task to a different other task state
        public void RemoveTo(ListBox listTo, ListBox listFrom, string pictureName,Dictionary<string,DateTime> listName, Dictionary<string, DateTime> listNameRemove)
        {
            try
            {
                ListBoxItem selectedItem = listTo.SelectedItem as ListBoxItem;
                if (selectedItem != null)
                {
                    foreach (var item in listNameRemove)
                    {
                        if (item.Key == selectedItem.Content.ToString())
                        {
                            listName.Add(item.Key,item.Value);
                            listNameRemove.Remove(item.Key) ;
                        }
                    }
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
            RemoveTo(pending, in_progress, "progress",TaskListProgress, TaskListPending);
        }
        // Button for removing select task from progress to pending states
        private void toPendingBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(in_progress, pending, "pending", TaskListPending, TaskListProgress);
        }
        // Button for removing select task from progress to completed states
        private void toCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(in_progress, completed, "completed", taskListComplete, TaskListProgress);
        }
        // Button for removing select task from completed to progress states
        private void toProgressFromCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTo(completed, in_progress, "progress", TaskListProgress, taskListComplete);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var item in TaskListProgress)
            {
                if (item.Value == DateTime.Now || item.Value < DateTime.Now)
                {
                    AddTask(overdue, item.Key, "overdue");
                    for (int n = in_progress.Items.Count - 1; n >= 0; --n)
                    {
                        if (in_progress.Items[n].ToString().Contains(item.Key))
                        {
                            TaskListProgress.Remove(item.Key);
                            in_progress.Items.RemoveAt(n);
                        }
                    }
                }
            }
        }

        public void DeleteTask(ListBox list,Dictionary<string,DateTime> dict)
        {
            ListBoxItem selectedItem = list.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                foreach (var item in dict)
                {
                    if (item.Key == selectedItem.Content.ToString())
                    {
                        dict.Remove(item.Key);
                    }
                }
                list.Items.Remove(list.SelectedItem);
            }
        }

        private void deletePending_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask(pending, TaskListPending);
        }

        private void deleteComplete_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask(completed, TaskListComplete);
        }

        private void deleteOverdue_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask(overdue, TaskListOverdue);
        }
    }
}
