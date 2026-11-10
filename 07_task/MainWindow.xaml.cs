using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _07_task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Gmail(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        private void Button_ukr_net(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sorry only (Gmail)!!!");
            Login login = new Login();
            login.ShowDialog();

            this.Close();
        }

        private void Button_iCloud(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sorry only (Gmail)!!!");
            Login login = new Login();
            login.ShowDialog();

            this.Close();
        }
    }
}