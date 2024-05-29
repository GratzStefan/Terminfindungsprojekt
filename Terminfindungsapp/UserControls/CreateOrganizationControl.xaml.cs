using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateOrganizationControl.xaml
    /// </summary>
    public partial class CreateOrganizationControl : UserControl
    {
        public CreateOrganizationControl()
        {
            InitializeComponent();
        }

        // Click-Event on Button, which creates new Organization
        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Checks if OrganizationName is not empty
            if (txtName.Text != "")
            {
                // Request creates new Organization
                if (await APICall.PostAsync<Organization>($"http://localhost:8080/api/organization/create", new Organization(txtName.Text, User.GetInstance(null).ID)))
                {
                    // Cleanup on GUI
                    txtName.Name = "";
                    MessageBox.Show("Successful!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
            else
            {
                MessageBox.Show("Organizations must have a name!");
            }
        }
    }
}
