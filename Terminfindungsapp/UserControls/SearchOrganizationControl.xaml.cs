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
using Terminfindungsapp.Entities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Terminfindungsapp.UserControls
{
    /// <summary>
    /// Interaction logic for SearchOrganizationControl.xaml
    /// </summary>
    public partial class SearchOrganizationControl : UserControl
    {
        private List<PostOrganization> organizations = new List<PostOrganization>();
        public SearchOrganizationControl()
        {
            InitializeComponent();
        }

        private async void txtOrganizationName_TextChanged(object sender, TextChangedEventArgs e)
        {
            staOrganization.Children.Clear();
            if (txtOrganizationName.Text == "")
                return;

            organizations = await APICall.RunAsync<List<PostOrganization>>($"http://localhost:8080/api/organization/search/{txtOrganizationName.Text}", null);

            if (organizations is not null)
            {
                staOrganization.Children.Clear();
                foreach (PostOrganization org in organizations)
                {
                    Grid grid = new Grid();
                    Color color = (Color)ColorConverter.ConvertFromString("#778899FF");
                    grid.Background = new SolidColorBrush(color);
                    grid.Margin = new Thickness(10, 10, 0, 10);

                    ColumnDefinition column1 = new ColumnDefinition();
                    ColumnDefinition column2 = new ColumnDefinition();
                    column1.Width = new GridLength(1, GridUnitType.Star);
                    column2.Width = GridLength.Auto;

                    // Add ColumnDefinitions to the Grid
                    grid.ColumnDefinitions.Add(column1);
                    grid.ColumnDefinitions.Add(column2);

                    //Labe for org
                    Label label = new Label();
                    label.Content = org.name;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.FontSize = 18;

                    // Create the Image
                    Button button = new Button();
                    button.HorizontalAlignment = HorizontalAlignment.Right;
                    button.Margin = new Thickness(5, 0, 15, 0);
                    button.Click += btnRequest_Click;

                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("/UserControls/request.png", UriKind.Relative));
                    image.Width = 24;
                    image.Height = 24;
                    //image.Margin = new Thickness(5, 0, 15, 0);
                    button.Content = image;

                    // Add the Image and TextBlock to the Grid
                    Grid.SetColumn(label, 0); // Set the column index for the Image
                    Grid.SetColumn(image, 1); // Set the column index for the TextBlock
                    grid.Children.Add(label);
                    grid.Children.Add(button);

                    staOrganization.Children.Add(grid);
                }
            }
        }
        private async void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Grid grid = btn.Parent as Grid;
            Label lbl = grid.Children[0] as Label;

            PostRequest postRequest = new PostRequest();
            postRequest.user = User.GetInstance(null);

            foreach (PostOrganization org in organizations)
            {
                if (org.name == lbl.Content.ToString())
                {
                    postRequest.org = org;
                    break;
                }
            }

            if(await APICall.PostAsync<PostRequest>($"http://localhost:8080/api/request/send", postRequest))
            {
                MessageBox.Show("Sent successfully!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
        }
    }
}
