using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Terminfindungsapp.Entities;

namespace Terminfindungsapp.UserControls
{
    /// <summary>
    /// Interaction logic for SearchOrganizationControl.xaml
    /// </summary>
    public partial class SearchOrganizationControl : UserControl
    {
        // Displayed List of searched Organizations
        private List<Organization> organizations = new List<Organization>();
        public SearchOrganizationControl()
        {
            InitializeComponent();
        }

        // Text-Changed of SearchBar (Requests entered Organizations)
        private async void txtOrganizationName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Clear GUI
            staOrganization.Children.Clear();

            // Checks if Organization is empty
            if (txtOrganizationName.Text == "")
                return;

            // Gets searched Organizations
            organizations = await APICall.GetAsync<List<Organization>>($"http://localhost:8080/api/organization/search/{txtOrganizationName.Text}", null);

            // Displays If entered Organizations exist
            if (organizations is not null)
            {
                // Clear GUI (once again) so values do not get overwritten by different Search
                staOrganization.Children.Clear();

                foreach (Organization org in organizations)
                {
                    // Creating GUI-Organization-Element

                    Color color = (Color)ColorConverter.ConvertFromString("#CAC0B3");

                    // Border around Element
                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush(color);
                    border.BorderThickness = new Thickness(2);
                    border.Margin = new Thickness(10);

                    Grid grid = new Grid
                    {
                        Background = new SolidColorBrush(Colors.Transparent),
                        Margin = new Thickness(20)
                    };

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
                    label.FontSize = 20;
                    label.FontWeight = FontWeights.Bold;
                    label.Foreground = new SolidColorBrush(color);

                    // Create the Image
                    Button button = new Button();
                    button.HorizontalAlignment = HorizontalAlignment.Right;
                    button.Background = new SolidColorBrush(Colors.Transparent);
                    button.BorderThickness = new Thickness();
                    button.Margin = new Thickness(5, 0, 15, 0);
                    button.Click += btnRequest_Click;

                    // Create a Grid to hold the rectangles
                    Grid gridImage = new Grid();

                    string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserControls/Images/request.png");

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

                    button.Content = gridImage;

                    // Add the Image and TextBlock to the Grid
                    Grid.SetColumn(label, 0);
                    Grid.SetColumn(button, 1);
                    grid.Children.Add(label);
                    grid.Children.Add(button);

                    
                    border.Child = grid;

                    // Add To GUI
                    staOrganization.Children.Add(border);
                }
            }
        }

        // Click-Event on Button of Organization, which sends Request to Organization
        private async void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            // Finding out OrganizationName
            Button btn = sender as Button;
            Grid grid = btn.Parent as Grid;
            Label lbl = grid.Children[0] as Label;

            // Create Request-Object
            Request postRequest = new Request()
            {
                user = User.GetInstance(null)
            };

            // Searching Organization out of List
            foreach (Organization org in organizations)
            {
                // Assigns Organization for Request of clicked Organization
                if (org.name == lbl.Content.ToString())
                {
                    postRequest.org = org;
                    break;
                }
            }


            // Request Creates new Request sent to Clicked Organization  
            if(await APICall.PostAsync<Request>($"http://localhost:8080/api/request/send", postRequest))
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
