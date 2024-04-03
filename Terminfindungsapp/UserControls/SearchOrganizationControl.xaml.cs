using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Terminfindungsapp.UserControls
{
    /// <summary>
    /// Interaction logic for SearchOrganizationControl.xaml
    /// </summary>
    public partial class SearchOrganizationControl : UserControl
    {
        public SearchOrganizationControl()
        {
            InitializeComponent();
        }

        private async void txtOrganizationName_TextChanged(object sender, TextChangedEventArgs e)
        {
            staOrganization.Children.Clear();
            if (txtOrganizationName.Text == "")
                return;

            List<PostOrganization> organizations = await APICall.RunAsync<List<PostOrganization>>($"http://localhost:8080/api/organization/search/{txtOrganizationName.Text}", null);

            if(organizations is not null)
            {
                foreach (PostOrganization org in organizations)
                {
                    Label label = new Label();
                    label.Content = org.name;
                    label.Width = 100;
                    
                    staOrganization.Children.Add(label);

                    //TODO: Organizations are Click-able (Linking to their Organization
                }
            }
        }
    }
}
