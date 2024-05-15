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
        private bool isInputPanelVisible = false;

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
                    stack.Margin = new Thickness { Top=10, Left=10, Bottom=10, Right=10 };

                    Label lblName = new Label();
                    lblName.Content = ev.titel;
                    lblName.FontWeight = FontWeights.Bold;
                    
                    stack.Children.Add(lblName);

                    Label lblDescription = new Label();
                    if(ev.description != null || ev.description != "")
                    {
                        lblDescription.Content = ev.description;
                    }
                    stack.Children.Add(lblDescription);

                    Grid.SetColumn(stack, 0);
                    grid.Children.Add(stack);

                    Label lblTimeline = new Label();
                    lblTimeline.Content = ev.datetimestart.ToString("dd. MMMM yyyy, HH:mm") + " - " + ev.datetimeend.ToString("dd. MMMM yyyy, HH:mm");
                    lblTimeline.VerticalContentAlignment = VerticalAlignment.Center;
                    lblTimeline.Margin = new Thickness { Right=5 };
                    lblTimeline.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(lblTimeline, 1);
                    grid.Children.Add(lblTimeline);

                    staEvents.Children.Add(grid);
                }
            }
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
