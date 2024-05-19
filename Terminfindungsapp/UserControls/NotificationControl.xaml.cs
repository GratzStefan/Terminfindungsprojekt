using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Terminfindungsapp.Entities;

namespace Terminfindungsapp.UserControls
{
    /// <summary>
    /// Interaktionslogik für NotificationControl.xaml
    /// </summary>
    public partial class NotificationControl : UserControl
    {
        private PostOrganization org;
        private List<PostRequest> requests = new List<PostRequest>();

        public NotificationControl(PostOrganization org)
        {
            InitializeComponent();
            this.org = org;
            DisplayRequests();
        }

        private async void DisplayRequests()
        {
            staRequests.Children.Clear();

            requests = await APICall.RunAsync<List<PostRequest>>($"http://localhost:8080/api/request/findToOrganization/{org.id}", null);

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

                    //Labe for org
                    Label label = new Label();
                    label.Content = req.org.name;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.FontSize = 18;

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
                    accept.HorizontalAlignment = HorizontalAlignment.Right;
                    accept.Margin = new Thickness(5, 0, 15, 0);
                    accept.Click += btnAccept_Click;

                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("/UserControls/tick.png", UriKind.Relative));
                    image.Width = 24;
                    image.Height = 24;
                    accept.Content = image;
                    Grid.SetColumn(accept, 0);
                    buttons.Children.Add(accept);

                    //Reject Button
                    Button reject = new Button();
                    reject.HorizontalAlignment = HorizontalAlignment.Right;
                    reject.Margin = new Thickness(5, 0, 15, 0);
                    reject.Click += btnReject_Click;

                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("/UserControls/cross.png", UriKind.Relative));
                    img.Width = 24;
                    img.Height = 24;
                    reject.Content = img;
                    Grid.SetColumn(reject, 1);
                    buttons.Children.Add(reject);




                    Grid.SetColumn(label, 0);
                    Grid.SetColumn(buttons, 1);


                    grid.Children.Add(label);
                    grid.Children.Add(buttons);

                    staRequests.Children.Add(grid);
                }
            }
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            PostRequest request = FindRequest(sender);

            if(request != null)
            {
                request.status = RequestStatus.ACCEPTED;
                ChangeStatus(request);
            }

        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            PostRequest request = FindRequest(sender);

            if (request != null)
            {
                request.status = RequestStatus.REJECTED;
                ChangeStatus(request);
            }
        }

        private async void ChangeStatus(PostRequest request)
        {
            if(await APICall.PutAsync($"http://localhost:8080/api/request/changeStatus?adminid={User.GetInstance(null).ID}", request))
            {
                DisplayRequests();
                MessageBox.Show("Successfully modified Status!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private PostRequest FindRequest(object sender)
        {
            Button btn = sender as Button;
            Grid btnGrid = btn.Parent as Grid;
            Grid grid = btnGrid.Parent as Grid;
            Label lbl = grid.Children[0] as Label;

            PostRequest request = null;

            foreach (PostRequest r in requests)
            {
                if (r.org.name == lbl.Content.ToString())
                {
                    request = r;
                    break;
                }
            }

            return request;
        }
    }
}
