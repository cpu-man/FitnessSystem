using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
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

namespace FitnessProgram
{
    /// <summary>
    /// Interaction logic for MemberTestWindow.xaml
    /// </summary>
    public partial class ActivityWindow : Window
    {
        Fitness fitness = new Fitness();
        public ActivityWindow()
        {
            InitializeComponent();
            ShowActivity();
        }

        public void ShowActivity()
        {
            string filePath = @"ActivityList.txt";
            string fileContent = File.ReadAllText(filePath);
            ActivityBlock.Text = fileContent;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NextWindow next = new NextWindow();
            next.Show();
            this.Close();
        }
    }
}
