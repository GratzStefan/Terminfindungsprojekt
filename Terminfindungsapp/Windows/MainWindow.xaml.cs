using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            //lblUsername.Content = User.GetInstance(null).Username;
            contentControl.Content = new OrganizationControl();
            /*List<PostOrganization> organizations = await APICall.RunAsync<List<PostOrganization>>($"http://localhost:8080/api/organization/search/{txtOrganizationName.Text}", null);
            foreach(PostOrganization org in organizations)
            {

            }*/
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

        }
    }
}
