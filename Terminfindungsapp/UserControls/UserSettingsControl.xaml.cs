using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Terminfindungsapp.Entities;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Terminfindungsapp.UserControls
{
    /// <summary>
    /// Interaction logic for UserSettingsControl.xaml
    /// </summary>
    public partial class UserSettingsControl : UserControl
    {
        // Displays List of sent Request of User
        private List<Request> requests = new List<Request>();
        
        public UserSettingsControl()
        {
            InitializeComponent();

            // Displays Requests
            DisplayRequests();

            // Displays Userinformation
            txtUsername.Text = User.GetInstance(null).Username;
            txtFirstname.Text = User.GetInstance(null).Firstname;
            txtLastname.Text = User.GetInstance(null).Lastname;
            
        }

        // Click-Event on Change Button (Changes User Information
        private async void modifyUser_Click(object sender, RoutedEventArgs e)
        {
            // Create User-Object
            User user = new User()
            {
                ID = User.GetInstance(null).ID,
                Username = txtUsername.Text,
                Firstname = txtFirstname.Text,
                Lastname = txtLastname.Text
            };

            // Request changes User-Information in DB
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
            // Reset GUI
            staRequests.Children.Clear();

            // GET all requests
            requests = await APICall.GetAsync<List<Request>>($"http://localhost:8080/api/request/findOfUser/{User.GetInstance(null).ID}", null);

            // Displays if requests exists
            if (requests is not null)
            {
                foreach (Request req in requests)
                {
                    // Creating GUI-Request-Element

                    // Border around GUI-Request
                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                    border.BorderThickness = new Thickness(2);
                    border.Margin = new Thickness(10);


                    Grid grid = new Grid();
                    grid.Background = new SolidColorBrush(Colors.Transparent);
                    grid.Margin = new Thickness(10, 10, 0, 10);

                    ColumnDefinition column1 = new ColumnDefinition();
                    ColumnDefinition column2 = new ColumnDefinition();
                    column1.Width = new GridLength(1, GridUnitType.Star);
                    column2.Width = GridLength.Auto;

                    grid.ColumnDefinitions.Add(column1);
                    grid.ColumnDefinitions.Add(column2);

                    // Label for org
                    Label label = new Label();
                    label.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                    label.Content = req.org.name;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.FontSize = 20;
                    label.FontWeight = FontWeights.Bold;
                    label.Margin = new Thickness
                    {
                        Bottom = 10,
                        Top = 10
                    };

                    // Create a Grid to hold the rectangles
                    Grid gridImage = new Grid();
                    gridImage.Margin = new Thickness(15);
                        
                    // Detect, which image needed
                    string imgUrl = null;
                    switch (req.status)
                    {
                        case RequestStatus.WAITING:
                            imgUrl = "UserControls/Images/hourglass.png";
                            break;
                        case RequestStatus.REJECTED:
                            imgUrl = "UserControls/Images/cross.png";
                            break;
                        case RequestStatus.ACCEPTED:
                            imgUrl = "UserControls/Images/tick.png";
                            break;
                    }

                    // Assigns Image
                    string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imgUrl);

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(absolutePath, UriKind.Absolute);
                    bitmapImage.EndInit();

                    // Rectangle with image background
                    Rectangle imageRectangle = new Rectangle
                    {
                        Width = 34,
                        Height = 34
                    };
                    ImageBrush imageBrush = new ImageBrush
                    {
                        ImageSource = bitmapImage
                    };
                    imageRectangle.Fill = imageBrush;

                    // Rectangle with color filter
                    Rectangle colorFilterRectangle = new Rectangle
                    {
                        Width = 34,
                        Height = 34,
                        Opacity = 1.0
                    };
                    SolidColorBrush colorBrush = new SolidColorBrush
                    {
                        Color = Color.FromRgb(202, 192, 179)
                    };
                    colorFilterRectangle.Fill = colorBrush;
                    ImageBrush opacityMaskBrush = new ImageBrush
                    {
                        ImageSource = bitmapImage
                    };
                    colorFilterRectangle.OpacityMask = opacityMaskBrush;

                    // Add rectangles to the Grid
                    gridImage.Children.Add(imageRectangle);
                    gridImage.Children.Add(colorFilterRectangle);

                    
                    // Add to Grid
                    Grid.SetColumn(label, 0);
                    Grid.SetColumn(gridImage, 1);
                    grid.Children.Add(label);
                    grid.Children.Add(gridImage);


                    border.Child = grid;


                    // Add to GUI
                    staRequests.Children.Add(border);
                }
            }
        }
    }
}
