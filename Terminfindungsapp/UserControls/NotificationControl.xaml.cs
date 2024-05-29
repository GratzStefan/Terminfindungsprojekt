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
    /// Interaktionslogik für NotificationControl.xaml
    /// </summary>
    public partial class NotificationControl : UserControl
    {
        // Current Organization
        private Organization org;
        
        // Displayed List of Sent Request of User 
        private List<Request> requests = new List<Request>();

        public NotificationControl(Organization org)
        {
            InitializeComponent();
            this.org = org;
            // Displaying Requests
            DisplayRequests();
        }

        private async void DisplayRequests()
        {
            // Clear GUI
            staRequests.Children.Clear();

            // Request gets all Sent Request of User
            requests = await APICall.GetAsync<List<Request>>($"http://localhost:8080/api/request/findToOrganization/{org.id}", null);

            // Displays Requests if any exist
            if (requests is not null)
            {
                foreach (Request req in requests)
                {
                    // Border
                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                    border.BorderThickness = new Thickness(2);
                    border.Margin = new Thickness(10);

                    Grid grid = new Grid();
                    grid.Background = new SolidColorBrush(Colors.Transparent);
                    grid.Margin = new Thickness(10);

                    ColumnDefinition column1 = new ColumnDefinition();
                    ColumnDefinition column2 = new ColumnDefinition();
                    column1.Width = new GridLength(1, GridUnitType.Star);
                    column2.Width = GridLength.Auto;

                    // Add ColumnDefinitions to the Grid
                    grid.ColumnDefinitions.Add(column1);
                    grid.ColumnDefinitions.Add(column2);

                    // Label for org
                    Label label = new Label();
                    label.Content = req.user.Username;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.FontSize = 20;
                    label.FontWeight = FontWeights.Bold;
                    label.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));

                    // Create the Image
                    Grid buttons = new Grid();
                   
                    ColumnDefinition acceptCol = new ColumnDefinition();
                    ColumnDefinition rejectCol = new ColumnDefinition();
                    acceptCol.Width = new GridLength(50, GridUnitType.Star);
                    rejectCol.Width = new GridLength(50, GridUnitType.Star);
                    buttons.ColumnDefinitions.Add(acceptCol);
                    buttons.ColumnDefinitions.Add(rejectCol);
                    
                    //Accept Button
                    Button accept = new Button();
                    accept.Background = new SolidColorBrush(Colors.Transparent);
                    accept.BorderThickness = new Thickness(0);
                    accept.HorizontalAlignment = HorizontalAlignment.Right;
                    accept.Margin = new Thickness(5, 0, 15, 0);
                    accept.Click += btnAccept_Click;
                    accept.Content = CreateImage("UserControls/Images/tick.png");
                    
                    Grid.SetColumn(accept, 0);
                    buttons.Children.Add(accept);

                    //Reject Button
                    Button reject = new Button();
                    reject.Background = new SolidColorBrush(Colors.Transparent);
                    reject.BorderThickness = new Thickness(0);
                    reject.HorizontalAlignment = HorizontalAlignment.Right;
                    reject.Margin = new Thickness(5, 0, 15, 0);
                    reject.Click += btnReject_Click;
                    reject.Content = CreateImage("UserControls/Images/cross.png");
                    
                    Grid.SetColumn(reject, 1);
                    buttons.Children.Add(reject);

                    // Add to Grid
                    Grid.SetColumn(label, 0);
                    Grid.SetColumn(buttons, 1);
                    grid.Children.Add(label);
                    grid.Children.Add(buttons);

                    border.Child = grid;

                    // Add to GUI
                    staRequests.Children.Add(border);
                }
            }
        }

        // Creates Image, which has a different Color
        private Grid CreateImage(string url)
        {
            // Create a Grid to hold the rectangles
            Grid gridImage = new Grid();

            string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, url);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(absolutePath, UriKind.Absolute);
            bitmapImage.EndInit();

            // Rectangle with image background
            Rectangle imageRectangle = new Rectangle
            {
                Width = 36,
                Height = 36
            };
            ImageBrush imageBrush = new ImageBrush
            {
                ImageSource = bitmapImage
            };
            imageRectangle.Fill = imageBrush;

            // Rectangle with color filter
            Rectangle colorFilterRectangle = new Rectangle
            {
                Width = 36,
                Height = 36,
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

            return gridImage;
        }

        // Click-Event on Accept-Button, which Accepts Request to Organization
        private void btnAccept_Click(object sender, EventArgs e)
        {
            // Finds Request
            Request request = FindRequest(sender);

            // Sends Request
            if(request != null)
            {
                request.status = RequestStatus.ACCEPTED;
                ChangeStatus(request);
            }

        }

        // Click-Event on Reject-Button, which Rejects Request to Organization
        private void btnReject_Click(object sender, EventArgs e)
        {
            // Finds Request
            Request request = FindRequest(sender);

            // Sends Request
            if (request != null)
            {
                request.status = RequestStatus.REJECTED;
                ChangeStatus(request);
            }
        }

        // Sends the Request with the new Status
        private async void ChangeStatus(Request request)
        {
            // PUT-Request
            if(await APICall.PutAsync($"http://localhost:8080/api/request/changeStatus?adminid={User.GetInstance(null).ID}", request))
            {
                // Resetting GUI
                DisplayRequests();
                MessageBox.Show("Successfully modified Status!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private Request FindRequest(object sender)
        {    
            Button btn = sender as Button;
            Grid btnGrid = btn.Parent as Grid;
            Grid grid = btnGrid.Parent as Grid;
            Label lbl = grid.Children[0] as Label;

            Request request = null;

            // Searches after clicked Request
            foreach (Request r in requests)
            {
                if (r.user.Username == lbl.Content.ToString())
                {
                    request = r;
                    break;
                }
            }

            return request;
        }
    }
}
