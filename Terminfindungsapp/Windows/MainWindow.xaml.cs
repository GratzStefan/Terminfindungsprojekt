using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Terminfindungsapp.UserControls;

namespace Terminfindungsapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Displayed List of Organizations
        private List<Organization> postOrganizations;

        public MainWindow()
        {
            InitializeComponent();

            // Setting default UserControl
            contentControl.Content = new HomepageControl();
            
            // Display Username
            lblUsername.Content = User.GetInstance(null).Username;
            
            // Loading Organizations of User 
            LoadOrganizationsOfUser();
        }


        private async void LoadOrganizationsOfUser()
        {
            // Request to API for Organizations of User
            postOrganizations = await APICall.GetAsync<List<Organization>>($"http://localhost:8080/api/organization/searchOrganizations/{User.GetInstance(null).ID}", null);
            
            // If there are organizations then they are getting displayed
            if(postOrganizations != null)
            {
                foreach (Organization org in postOrganizations)
                {
                    // Create Organization-Button
                    Button btnOrganization = new Button();
                    btnOrganization.Content = org.name;
                    btnOrganization.Margin = new Thickness
                    {
                        Top = 10
                    };
                    btnOrganization.Padding = new Thickness(10);
                    btnOrganization.HorizontalContentAlignment = HorizontalAlignment.Left;

                    Color color = (Color)ColorConverter.ConvertFromString("#CAC0B3");
                    btnOrganization.BorderBrush = new SolidColorBrush(color);
                    btnOrganization.Foreground = new SolidColorBrush(color);
                    btnOrganization.Background = new SolidColorBrush(Colors.Transparent);
                    btnOrganization.FontSize = 20;
                    btnOrganization.FontWeight = FontWeights.Bold;

                    btnOrganization.Click += btnOrganization_Click;
                    staUserOrganizations.Children.Add(btnOrganization);
                }
            }
        }

        // Navigates to Organization-UserControl when clicked on specific Organization
        private void btnOrganization_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            // Detecting organization
            Organization clickedOrganization = postOrganizations.FirstOrDefault(org => org.name == clickedButton.Content.ToString());
            // Opening selected Organization
            contentControl.Content = new OrganizationControl(clickedOrganization);  
        }

        // Navigates To CreateOrganization-UserControl
        private void btnCreateOrganization_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new CreateOrganizationControl();
        }

        // Navigates To SearchOrganization-UserControl
        private void btnSearchOrganization_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new SearchOrganizationControl();
        }

        // Navigates To Homepage-UserControl
        private void btnHomepage_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new HomepageControl();
        }

        // Navigates To UserSettings-UserControl
        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new UserSettingsControl();
        }

        // Navigates To LoginPageWindow and logs User out
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            User.GetInstance(new User());
            LoginWindow login = new LoginWindow();
            login.Show();

            this.Close();
        }
    }
}
