using System;
using System.Collections.Generic;
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

namespace Terminfindungsapp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // Click-Event on Login-Button (Validates if username and password is valid)
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Input-Fields
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Gets Return of User If Worked
            User user = await APICall.GetAsync<User>($"http://localhost:8080/api/user/login?username={username}&password={password}", null);
            
            if (user == null)
            {
                return;
            }

            // Assigns Current User globally
            User.GetInstance(user);

            // Navigates to MainWindow
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        // Navigates to RegisterWindow
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Close();
        }
    }
}
