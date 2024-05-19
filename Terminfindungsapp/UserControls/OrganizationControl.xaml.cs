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
        private PostOrganization org;
        public OrganizationControl(PostOrganization org)
        {
            InitializeComponent();
            this.org = org;
            lblName.Content = org.name;
            contentControl.Content = new DashboardControl(org);
        }

        

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new DashboardControl(org);
        }

        private void btnNotifcation_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new NotificationControl(org);
        }
    }
}
