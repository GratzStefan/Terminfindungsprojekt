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
        private List<PostOrganization> postOrganizations;

        public MainWindow()
        {
            InitializeComponent();

            //contentControl.Content = new OrganizationControl();
            User user = new User();
            user.ID = "663519a065014269ff6d96ac";
            user.Username = "test";
            User.GetInstance(user);
            
            lblUsername.Text = User.GetInstance(null).Username;
            LoadOrganizationsOfUser();
        }

        private async void LoadOrganizationsOfUser()
        {
            postOrganizations = await APICall.RunAsync<List<PostOrganization>>($"http://localhost:8080/api/organization/searchOrganizations/{User.GetInstance(null).ID}", null);
            if(postOrganizations != null)
            {
                foreach (PostOrganization org in postOrganizations)
                {
                    Button btnOrganization = new Button();
                    btnOrganization.Content = org.name;
                    btnOrganization.Margin = new Thickness
                    {
                        Top = 10
                    };
                    Color color = (Color)ColorConverter.ConvertFromString("#778899FF");
                    btnOrganization.Background = new SolidColorBrush(color);
                    btnOrganization.Click += btnOrganization_Click;
                    staUserOrganizations.Children.Add(btnOrganization);
                }
            }
        }

        private void btnOrganization_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            PostOrganization clickedOrganization = postOrganizations.FirstOrDefault(org => org.name == clickedButton.Content.ToString());
            contentControl.Content = new OrganizationControl(clickedOrganization);  
        }

        private void btnCreateOrganization_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new CreateOrganizationControl();
        }

        private void btnSearchOrganization_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new SearchOrganizationControl();
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new UserSettingsControl();
        }
    }
}
