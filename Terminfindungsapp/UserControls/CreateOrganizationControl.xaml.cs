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

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if(await APICall.PostAsync<PostOrganization>($"http://localhost:8080/api/organization/create", new PostOrganization(txtName.Name, User.GetInstance(null).ID)))
            {
                MessageBox.Show("Successful!");
                //TODO: Change to Organization-Perspective
            }
            else
            {
                MessageBox.Show("Unsuccessful!");
                //TODO: Display Reason, why request was unsuccessful
            }
        }
    }
}
