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

namespace FitnessProgram
{
    /// <summary>
    /// Interaction logic for NextWindow.xaml
    /// </summary>
    public partial class NextWindow : Window
    {
        public NextWindow()
        {
            InitializeComponent();
        }

        private void GoToMembers_Click(object sender, RoutedEventArgs e)
        {
            MemberWindow member = new MemberWindow();
            member.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < 1000; i++)
            {
                MainWindow main = new MainWindow();
                main.Show();
            }
        }

        private void GoToActivity_Click(object sender, RoutedEventArgs e)
        {
            ActivityWindow activity = new ActivityWindow();
            activity.Show();
            this.Close();
        }
    }
}
