using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        // When Clicked On Register (Creates new User)
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Input-Fields
            string firstname = txtFirstname.Text;
            string lastname = txtLastname.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Checks if Username and password is long enough
            if (username.Count() > 4 && password.Count() > 8)
            {
                // Request to Server To Create new User
                if(await APICall.PostAsync<User>("http://localhost:8080/api/user/signup", new User(username, password, firstname, lastname)))
                {
                    MessageBox.Show("Registration successful!");
                }
                else
                {
                    MessageBox.Show("Registration something went wrong!");

                }
            }
            else
            {
                MessageBox.Show("Username or password is too short!");
            }
        }

        // Navigates to LoginWindow
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
