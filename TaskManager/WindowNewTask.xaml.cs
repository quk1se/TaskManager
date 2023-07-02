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
using System.Windows.Shapes;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для WindowNewTask.xaml
    /// </summary>
    public partial class WindowNewTask : Window
    {
        public string taskNameTxt;
        public DateTime taskDeadline;
        public bool is_click = false;
        public WindowNewTask()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox.Text == "")
            {
                MessageBox.Show("Enter name!");
                return;
            }
            taskNameTxt = txtBox.Text;
            DateTime.TryParse(deadLine.Text, out taskDeadline);
            MessageBox.Show(taskDeadline.ToString());
            is_click = true;
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
