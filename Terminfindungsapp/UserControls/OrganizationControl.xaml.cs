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
            DisplayEvents();
        }

        private async void DisplayEvents()
        {
            List<PostEvent> events = await APICall.RunAsync<List<PostEvent>>($"http://localhost:8080/api/events/search/{org.id}", null);
            if(events is not null)
            {
                foreach (PostEvent ev in events)
                {
                    Button btn = new Button();
                    btn.Content = ev.titel + ";" + ev.description + ";" + ev.datetime;
                    staEvents.Children.Add(btn);
                }
            }
        }

        private async void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if(await APICall.PutAsync<PostOrganization>($"http://localhost:8080/api/organization/addUser?userid={txtUserID.Text}&organizationid={org.id}&adminid={User.GetInstance(null).ID}"))
            {
                MessageBox.Show("Added user");
            }
            else
            {
                MessageBox.Show("Failed!");
            }
        }

        private async void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (await APICall.PostAsync<PostEvent>("http://localhost:8080/api/events/add", new PostEvent("Titel", "Description", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), org.id)))
            {
                MessageBox.Show("Added event");
            }
            else 
            {
                MessageBox.Show("Failed!");
            }
        }
    }
}
