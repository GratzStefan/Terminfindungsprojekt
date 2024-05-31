using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
using Terminfindungsapp.Entities;
using Terminfindungsapp.UserControls;

namespace Terminfindungsapp
{
    /// <summary>
    /// Interaction logic for OrganizationControl.xaml
    /// </summary>
    public partial class OrganizationControl : UserControl
    {
        // Current Organization
        private Organization org;

        public OrganizationControl(Organization org)
        {
            InitializeComponent();
            // Assigning Organization
            this.org = org;
            // Displaying OrganizationName
            lblName.Content = org.name;
            // Setting Default User-Control
            contentControl.Content = new DashboardControl(org);
        }
        
        // Navigate to Dashboard-UserControl
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new DashboardControl(org);
        }

        // Navigate to Notification-UserControl
        private void btnNotifcation_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new NotificationControl(org);
        }

        // Click Event on Delete-Button, which deletes current Organization
        private async void btnDeleteOrganization_Click(object sender, RoutedEventArgs e)
        {
            // Request, that deletes current Organization (returns how many organizations got deleted)
            if(await APICall.DeleteAsync<int>($"http://localhost:8080/api/organization/delete/{org.id}")==1)
            {
                MessageBox.Show("Successfully deleted organization!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
        }
    }
}
