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
    /// Interaction logic for UserSettingsControl.xaml
    /// </summary>
    public partial class UserSettingsControl : UserControl
    {
        private List<PostRequest> requests = new List<PostRequest>();
        public UserSettingsControl()
        {
            InitializeComponent();

            DisplayRequests();
            txtUsername.Text = User.GetInstance(null).Username;
            txtFirstname.Text = User.GetInstance(null).Firstname;
            txtLastname.Text = User.GetInstance(null).Lastname;
            
        }

        private async void modifyUser_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.ID = User.GetInstance(null).ID;
            user.Username = txtUsername.Text;
            user.Firstname = txtFirstname.Text;
            user.Lastname = txtLastname.Text;

            if (await APICall.PutAsync($"http://localhost:8080/api/user/modifyUser", user))
            {
                MessageBox.Show("Successfully changed User-Settings!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private async void DisplayRequests()
        {
            staRequests.Children.Clear();

            requests = await APICall.RunAsync<List<PostRequest>>($"http://localhost:8080/api/request/findOfUser/{User.GetInstance(null).ID}", null);

            if (requests is not null)
            {
                foreach (PostRequest req in requests)
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

                    // Label for org
                    Label label = new Label();
                    label.Content = req.org.name;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.FontSize = 18;
                    label.Margin = new Thickness
                    {
                        Bottom = 10,
                        Top = 10
                    };

                    // Create the Image
                    Image image = new Image();
                    string imgUrl = null;
                    switch (req.status)
                    {
                        case RequestStatus.WAITING:
                            imgUrl = "/UserControls/hourglass.png";
                            break;
                        case RequestStatus.REJECTED:
                            imgUrl = "/UserControls/cross.png";
                            break;
                        case RequestStatus.ACCEPTED:
                            imgUrl = "/UserControls/tick.png";
                            break;
                    }
                    image.Source = new BitmapImage(new Uri(imgUrl, UriKind.Relative));
                    image.Width = 24;
                    image.Height = 24;
                    image.Margin = new Thickness
                    {
                        Right = 10
                    };

                    Grid.SetColumn(label, 0);
                    Grid.SetColumn(image, 1);
                    grid.Children.Add(label);
                    grid.Children.Add(image);

                    staRequests.Children.Add(grid);
                }
            }
        }
    }
}
