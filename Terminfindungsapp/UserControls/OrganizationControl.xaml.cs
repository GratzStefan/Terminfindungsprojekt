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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Terminfindungsapp
{
    /// <summary>
    /// Interaction logic for OrganizationControl.xaml
    /// </summary>
    public partial class OrganizationControl : UserControl
    {
        private PostOrganization org;
        public OrganizationControl(PostOrganization org)
        {
            InitializeComponent();
            this.org = org;
            lblName.Content = org.name;
        }

        private async void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            await APICall.PostAsync<List<PostOrganization>>($"http://localhost:8080/api/organization/searchOrganizations/{User.GetInstance(null).ID}", null);
        }

        private void btnAddDate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
