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
        private PostOrganization org;
        private List<User> users = new List<User>();
        private bool isInputPanelVisible = false;

        public DashboardControl(PostOrganization org)
        {
            InitializeComponent();
            this.org = org;
            DisplayEvents();
            DisplayUsers();
        }

        private async void DisplayEvents()
        {
            staEvents.Children.Clear();
            List<PostEvent> events = await APICall.RunAsync<List<PostEvent>>($"http://localhost:8080/api/events/search/{org.id}", null);

            if (events is not null)
            {
                foreach (KeyValuePair<string, GroupedEvent> grouped in GroupEvents(events))
                {
                    Grid day = new Grid();
                    day.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    day.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Star) });


                    Label lblDay = new Label();
                    lblDay.Content = grouped.Key;
                    lblDay.FontWeight = FontWeights.Bold;
                    Grid.SetRow(lblDay, 0);
                    day.Children.Add(lblDay);



                    StackPanel eventsPanel = new StackPanel();
                    foreach (PostEvent ev in grouped.Value.events)
                    {
                        Grid grid = new Grid();
                        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50, GridUnitType.Star) });
                        grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
                        grid.Margin = new Thickness
                        {
                            Top = 10,
                            Bottom = 10,
                            Left = 10,
                            Right = 10
                        };

                        StackPanel stack = new StackPanel();
                        stack.Margin = new Thickness { Top = 10, Left = 10, Bottom = 10, Right = 10 };

                        Label lblName = new Label();
                        lblName.Content = ev.titel;
                        lblName.FontWeight = FontWeights.Bold;

                        stack.Children.Add(lblName);

                        Label lblDescription = new Label();
                        if (ev.description != null || ev.description != "")
                        {
                            lblDescription.Content = ev.description;
                        }
                        stack.Children.Add(lblDescription);

                        Grid.SetColumn(stack, 0);
                        grid.Children.Add(stack);

                        Label lblTimeline = new Label();
                        lblTimeline.Content = ev.datetimestart.ToString("dd. MMMM yyyy, HH:mm") + " - " + ev.datetimeend.ToString("dd. MMMM yyyy, HH:mm");
                        lblTimeline.VerticalContentAlignment = VerticalAlignment.Center;
                        lblTimeline.Margin = new Thickness { Right = 5 };
                        lblTimeline.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetColumn(lblTimeline, 1);
                        grid.Children.Add(lblTimeline);

                        eventsPanel.Children.Add(grid);
                    }
                    Grid.SetRow(eventsPanel, 1);
                    day.Children.Add(eventsPanel);

                    staEvents.Children.Add(day);
                }
            }
        }

        private Dictionary<string, GroupedEvent> GroupEvents(List<PostEvent> events)
        {
            Dictionary<string, GroupedEvent> grouped = new Dictionary<string, GroupedEvent>();
            foreach(PostEvent ev in events)
            {
                string datekey = ev.datetimestart.Date.ToString("dddd, MMMM d, yyyy");

                if (!grouped.ContainsKey(datekey))
                {
                    grouped[datekey] = new GroupedEvent(ev.datetimestart);
                }
                grouped[datekey].events.Add(ev);
                grouped[datekey].events = grouped[datekey].events.OrderBy(sort => sort.datetimestart).ToList();
            }

            return grouped;
        }

        private async void DisplayUsers()
        {
            staUsers.Children.Clear();
            users = await APICall.RunAsync<List<User>>($"http://localhost:8080/api/organization/userListOrganization/{org.id}", null);

            if (users is not null)
            {
                foreach (User user in users)
                {
                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Auto) });

                    grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
                    grid.Margin = new Thickness
                    {
                        Top = 10,
                        Bottom = 10,
                        Left = 10,
                        Right = 10
                    };

                    Label lblName = new Label();
                    lblName.Content = user.Username;
                    lblName.FontWeight = FontWeights.Bold;
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

                    Button btnModify = new Button();
                    Image imgModify = new Image();
                    imgModify.Source = new BitmapImage(new Uri("/UserControls/promote.png", UriKind.Relative));
                    imgModify.Width = 24;
                    imgModify.Height = 24;
                    btnModify.Content = imgModify;
                    btnModify.Margin = new Thickness
                    {
                        Top = 10,
                        Bottom = 10,
                        Right = 10,
                    };
                    btnModify.Click += modifyUser_Click;
                    Grid.SetColumn(btnModify, 0);
                    gridButtons.Children.Add(btnModify);

                    Button btnRemove = new Button();
                    Image imgRemove = new Image();
                    imgRemove.Source = new BitmapImage(new Uri("/UserControls/remove.png", UriKind.Relative));
                    imgRemove.Width = 24;
                    imgRemove.Height = 24;
                    btnRemove.Content = imgRemove;
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

                    staUsers.Children.Add(grid);
                }
            }
        }

        private async void modifyUser_Click(object sender, RoutedEventArgs e)
        {
            User user = FindUser(sender);
            
            if(user != null) {
                if(await APICall.PutAsync<PostOrganization>($"http://localhost:8080/api/organization/promote?orgid={org.id}&userid={user.ID}&adminid={User.GetInstance(null).ID}", null))
                {
                    MessageBox.Show("Successfully modified user!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
        }

        private async void removeUser_Click(object sender, RoutedEventArgs e)
        {
            User user = FindUser(sender);


            if (user != null)
            {
                if (await APICall.RemoveAsync<bool>($"http://localhost:8080/api/organization/removeUser?userid={user.ID}&orgid={org.id}&adminid={User.GetInstance(null).ID}"))
                {
                    users.Remove(user);
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


        private async void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            string titel = inpTitel.Text;
            string description = inpDescription.Text;

            string[] startdate = inpStartDate.Text.Split('.');
            string[] enddate = inpEndDate.Text.Split('.');
            if (titel == "" || startdate.Length != 3 || enddate.Length != 3 || inpStartHour.Text == "" || inpStartMin.Text == "" || inpEndHour.Text == "" || inpEndMin.Text == "")
            {
                MessageBox.Show("Input is invalid!");
                return;
            }

            if(description == "")
            {
                description = null;
            }

            try
            {
                DateTime start = new DateTime(Convert.ToInt32(startdate[2]), Convert.ToInt32(startdate[1]), Convert.ToInt32(startdate[0]), Convert.ToInt32(inpStartHour.Text), Convert.ToInt32(inpStartMin.Text), 0);
                DateTime end = new DateTime(Convert.ToInt32(enddate[2]), Convert.ToInt32(enddate[1]), Convert.ToInt32(enddate[0]), Convert.ToInt32(inpEndHour.Text), Convert.ToInt32(inpEndMin.Text), 0);

                if (start < end)
                {
                    if (await APICall.PostAsync<PostEvent>("http://localhost:8080/api/events/add", new PostEvent(titel, description, start, end, org.id)))
                    {
                        MessageBox.Show("Added event");
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