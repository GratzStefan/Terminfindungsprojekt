using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
    /// Interaktionslogik für DashboardControl.xaml
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        // Current Organization
        private Organization org;
        // Displayed List of OrganizationsUser
        private List<User> users = new List<User>();
        // Displayed List of OrganizationsEvents
        private List<Event> events = new List<Event>();
        // Is AddEvent Panel expanded
        private bool isInputPanelVisible = false;

        public DashboardControl(Organization org)
        {
            InitializeComponent();

            this.org = org;
            // Display Events of Organization
            DisplayEvents();
            // Display Users of Organization
            DisplayUsers();
        }

        private async void DisplayEvents()
        {
            // Clear Events GUI
            staEvents.Children.Clear();

            // Request, which gets Events of Organization
            events = await APICall.GetAsync<List<Event>>($"http://localhost:8080/api/events/search/{org.id}", null);

            // Creates GUI-Elements of Requests
            if (events is not null)
            {
                foreach (KeyValuePair<string, GroupedEvent> grouped in GroupEvents(events))
                {
                    Grid day = new Grid();
                    day.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    day.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Star) });
                    day.Margin = new Thickness(10);

                    // Label of Dy
                    Label lblDay = new Label();
                    lblDay.Content = grouped.Key;
                    lblDay.FontWeight = FontWeights.Bold;
                    lblDay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0c3185"));
                    lblDay.FontSize = 20;
                    Grid.SetRow(lblDay, 0);
                    day.Children.Add(lblDay);

                    // Request-GUI-Element
                    StackPanel eventsPanel = new StackPanel();

                    foreach (Event ev in grouped.Value.events)
                    {
                        // Border
                        Border border = new Border();
                        border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                        border.BorderThickness = new Thickness(2);
                        border.Margin = new Thickness(5);

                        Grid grid = new Grid();
                        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
                        grid.Background = new SolidColorBrush(Colors.Transparent);
                        grid.Margin = new Thickness
                        {
                            Top = 10,
                            Bottom = 10,
                            Left = 10,
                            Right = 10
                        };

                        StackPanel stack = new StackPanel();
                        stack.Margin = new Thickness { Top = 10, Left = 10, Bottom = 10, Right = 10 };

                        // Label of RequestName
                        Label lblName = new Label();
                        lblName.Content = ev.titel;
                        lblName.FontWeight = FontWeights.Bold;
                        lblName.FontSize = 20;
                        lblName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));

                        stack.Children.Add(lblName);

                        // Label of RequestDescription
                        Label lblDescription = new Label();
                        lblDescription.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                        lblDescription.FontSize = 18;

                        if (ev.description != null || ev.description != "")
                        {
                            lblDescription.Content = ev.description;
                        }
                        stack.Children.Add(lblDescription);

                        Grid.SetColumn(stack, 0);
                        grid.Children.Add(stack);

                        // Label of Timeline
                        Label lblTimeline = new Label();
                        lblTimeline.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                        lblTimeline.FontSize = 18;
                        lblTimeline.Content = ev.datetimestart.ToString("dd. MMMM yyyy, HH:mm") + " - " + ev.datetimeend.ToString("dd. MMMM yyyy, HH:mm");
                        lblTimeline.VerticalContentAlignment = VerticalAlignment.Center;
                        lblTimeline.Margin = new Thickness { Right = 5 };
                        lblTimeline.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetColumn(lblTimeline, 1);
                        grid.Children.Add(lblTimeline);

                        border.Child = grid;
                        eventsPanel.Children.Add(border);
                    }
                    Grid.SetRow(eventsPanel, 1);
                    day.Children.Add(eventsPanel);

                    // Add to GUI
                    staEvents.Children.Add(day);
                }
            }
        }

        private Dictionary<string, GroupedEvent> GroupEvents(List<Event> events)
        {
            Dictionary<string, GroupedEvent> grouped = new Dictionary<string, GroupedEvent>();
            
            // Groups Events by day
            foreach(Event ev in events)
            {
                // Gets day
                string datekey = ev.datetimestart.Date.ToString("dddd, MMMM d, yyyy");

                // Checks if day already exists, if not it creates day
                if (!grouped.ContainsKey(datekey))
                {
                    grouped[datekey] = new GroupedEvent(ev.datetimestart);
                }
                // Add event to day
                grouped[datekey].events.Add(ev);
                // Sort day
                grouped[datekey].events = grouped[datekey].events.OrderBy(sort => sort.datetimestart).ToList();
            }

            return grouped;
        }

        private async void DisplayUsers()
        {
            // Resets Users-GUI-Element
            staUsers.Children.Clear();
            // Request to get User of Organization
            users = await APICall.GetAsync<List<User>>($"http://localhost:8080/api/organization/userListOrganization/{org.id}", null);

            // Displays User in GUI
            if (users is not null)
            {
                foreach (User user in users)
                {
                    // Border
                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                    border.BorderThickness = new Thickness(2);
                    border.Margin = new Thickness
                    {
                        Top = 10,
                        Bottom = 10,
                        Left = 10,
                        Right = 10
                    };

                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Auto) });
                    grid.Background = new SolidColorBrush(Colors.Transparent);

                    // Label of UserName
                    Label lblName = new Label();
                    lblName.Content = user.Username;
                    lblName.FontWeight = FontWeights.Bold;
                    lblName.FontSize = 20;
                    lblName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAC0B3"));
                    lblName.VerticalContentAlignment = VerticalAlignment.Center;
                    lblName.Margin = new Thickness
                    {
                        Left = 10,
                    };
                    Grid.SetColumn(lblName, 0);
                    grid.Children.Add(lblName);


                    Grid gridButtons = new Grid();
                    gridButtons.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
                    gridButtons.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
                    Grid.SetColumn(gridButtons, 1);

                    // Button to Promote User to Admin
                    Button btnPromote = new Button();
                    btnPromote.Background = new SolidColorBrush(Colors.Transparent);
                    btnPromote.BorderThickness = new Thickness(0);
                    btnPromote.Content = CreateImage("UserControls/Images/promote.png");
                    btnPromote.Margin = new Thickness
                    {
                        Top = 10,
                        Bottom = 10,
                        Right = 10,
                    };
                    btnPromote.Click += modifyUser_Click;
                    Grid.SetColumn(btnPromote, 0);
                    gridButtons.Children.Add(btnPromote);

                    // Button to Remove User from Organization
                    Button btnRemove = new Button();
                    btnRemove.Background = new SolidColorBrush(Colors.Transparent);
                    btnRemove.BorderThickness = new Thickness(0);
                    btnRemove.Content = CreateImage("UserControls/Images/remove.png");
                    btnRemove.Margin = new Thickness
                    {
                        Top = 10,
                        Bottom = 10,
                        Right = 10,
                    };
                    btnRemove.Click += removeUser_Click;
                    Grid.SetColumn(btnRemove, 1);
                    gridButtons.Children.Add(btnRemove);

                    grid.Children.Add(gridButtons);

                    border.Child = grid;

                    // Add to GUI
                    staUsers.Children.Add(border);
                }
            }
        }

        // Creates Image of different Color
        private Grid CreateImage(string url)
        {
            // Create a Grid to hold the rectangles
            Grid gridImage = new Grid();

            // Get Image
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

        // Click-Event on Button, which promotes User to Admin
        private async void modifyUser_Click(object sender, RoutedEventArgs e)
        {
            // Finds User
            User user = FindUser(sender);
            
            if(user != null) {
                // Request to change Role
                if(await APICall.PutAsync<Organization>($"http://localhost:8080/api/organization/promote?orgid={org.id}&userid={user.ID}&adminid={User.GetInstance(null).ID}", null))
                {
                    MessageBox.Show("Successfully promoted user!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
        }

        // Click-Event on Button, which removes User from Organization
        private async void removeUser_Click(object sender, RoutedEventArgs e)
        {
            // Finds User
            User user = FindUser(sender);

            if (user != null)
            {
                // Request to Remove User
                if (await APICall.DeleteAsync<bool>($"http://localhost:8080/api/organization/removeUser?userid={user.ID}&orgid={org.id}&adminid={User.GetInstance(null).ID}"))
                {
                    // Remove user from list
                    users.Remove(user);
                    // Display new List of User
                    DisplayUsers();
                    MessageBox.Show("Successfully deleted user!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
        }

        private User FindUser(object sender)
        {
            Button btn = sender as Button;
            Grid btnGrid = btn.Parent as Grid;
            Grid grid = btnGrid.Parent as Grid;
            Label lbl = grid.Children[0] as Label;

            User user = null;

            // Searchs after User in List
            foreach (User u in users)
            {
                if (u.Username == lbl.Content.ToString())
                {
                    user = u;
                    break;
                }
            }

            return user;
        }

        // Animation for AddEvent-Button
        private void toggleButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle input panel visibility
            isInputPanelVisible = !isInputPanelVisible;

            if (isInputPanelVisible)
            {
                // Slide up animation
                DoubleAnimation slideUpAnimation = new DoubleAnimation(0, inputPanel.ActualHeight, TimeSpan.FromSeconds(0.3));
                inputPanel.BeginAnimation(HeightProperty, slideUpAnimation);
                inputPanel.Visibility = Visibility.Visible;
            }
            else
            {
                // Slide down animation
                DoubleAnimation slideDownAnimation = new DoubleAnimation(inputPanel.ActualHeight, 0, TimeSpan.FromSeconds(0.3));
                inputPanel.BeginAnimation(HeightProperty, slideDownAnimation);
                slideDownAnimation.Completed += (s, _) => inputPanel.Visibility = Visibility.Collapsed;
            }
        }

        // Click-Event on Button, which adds Event to Organization
        private async void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            // Input-Fields
            string titel = inpTitel.Text;
            string description = inpDescription.Text;
            string[] startdate = inpStartDate.Text.Split('.');
            string[] enddate = inpEndDate.Text.Split('.');

            // Checks if Input is valid
            if (titel == "" || startdate.Length != 3 || enddate.Length != 3 || inpStartHour.Text == "" || inpStartMin.Text == "" || inpEndHour.Text == "" || inpEndMin.Text == "")
            {
                MessageBox.Show("Input is invalid!");
                return;
            }

            // Set to null, so it does not appear in JSON
            if(description == "")
            {
                description = null;
            }

            try
            {
                // Timelines
                DateTime start = new DateTime(Convert.ToInt32(startdate[2]), Convert.ToInt32(startdate[1]), Convert.ToInt32(startdate[0]), Convert.ToInt32(inpStartHour.Text), Convert.ToInt32(inpStartMin.Text), 0);
                DateTime end = new DateTime(Convert.ToInt32(enddate[2]), Convert.ToInt32(enddate[1]), Convert.ToInt32(enddate[0]), Convert.ToInt32(inpEndHour.Text), Convert.ToInt32(inpEndMin.Text), 0);

                // Checks if DateTime end is not before the start
                if (start < end)
                {
                    // Request creates new Event on Organization
                    if (await APICall.PostAsync<Event>("http://localhost:8080/api/events/add", new Event(titel, description, start, end, org.id)))
                    {
                        MessageBox.Show("Added event");
                        // Displays Events in GUI
                        DisplayEvents();
                    }
                    else
                    {
                        MessageBox.Show("Failed!");
                    }
                }
            }
            catch(ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("DateTime invalid!");
            }
        }
    }
}