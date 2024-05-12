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
using Terminfindungsapp.Entities;

namespace Terminfindungsapp.UserControls
{
    /// <summary>
    /// Interaktionslogik für DashboardControl.xaml
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        private PostOrganization org;
        public DashboardControl(PostOrganization org)
        {
            InitializeComponent();
            this.org = org;
            DisplayEvents();
        }

        private async void DisplayEvents()
        {
            List<PostEvent> events = await APICall.RunAsync<List<PostEvent>>($"http://localhost:8080/api/events/search/{org.id}", null);
            if (events is not null)
            {
                foreach (PostEvent ev in events)
                {
                    Button btn = new Button();
                    btn.Content = ev.titel + ";" + ev.description + ";" + ev.datetimestart + ";" + ev.datetimeend;
                    staEvents.Children.Add(btn);
                }
            }
        }
        
        private async void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (await APICall.PostAsync<PostEvent>("http://localhost:8080/api/events/add", new PostEvent("Titel", "Description", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"), org.id)))
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
