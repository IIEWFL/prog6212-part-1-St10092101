using System.Windows;

namespace TimeManagementApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            // Check if Student number and password is entered

            if (string.IsNullOrWhiteSpace(StudentNo.Text) || string.IsNullOrWhiteSpace(Passwordbx.Password))
            {
                MessageBox.Show("Both username and password must be entered.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // If both student number and password is entered current window will close and following window will open

                Modules moduleWindow = new Modules();
                moduleWindow.Show();
                this.Close();
            }
        }
    }
}