using System;
using System.Windows;
using System.Windows.Input;

namespace ClientApp.Views
{

    public partial class AuthorizationView : Window
    {
        public AuthorizationView()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = loginTextBox.Text;
                var password = passswordTextBox.Text;

                if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException($"{nameof(login)} or/and {nameof(password)} is empty");
                }

                this.Hide();
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
