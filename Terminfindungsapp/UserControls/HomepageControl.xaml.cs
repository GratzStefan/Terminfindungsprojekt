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
    /// Interaktionslogik für HomepageControl.xaml
    /// </summary>
    public partial class HomepageControl : UserControl
    {
        // Displayed List of Events
        private List<Event> events = new List<Event>();
        
        public HomepageControl()
        {
            InitializeComponent();
            // Displays all Events of User
            DisplayEvents();
        }

        private async void DisplayEvents()
        {
            // Resets GUI
            staEvents.Children.Clear();
            // Request for all Event of User
            events = await APICall.GetAsync<List<Event>>($"http://localhost:8080/api/events/find/{User.GetInstance(null).ID}", null);

            // Displays Events if there are any
            if (events is not null)
            {
                // Goes through Events of each Day
                foreach (KeyValuePair<string, GroupedEvent> grouped in GroupEvents(events))
                {
                    Grid day = new Grid();
                    day.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    day.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Star) });

                    // Label day
                    Label lblDay = new Label();
                    lblDay.Content = grouped.Key;
                    lblDay.FontWeight = FontWeights.Bold;
                    lblDay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0c3185"));
                    lblDay.FontSize = 20;
                    Grid.SetRow(lblDay, 0);
                    day.Children.Add(lblDay);

                    // Request-Element
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
                        lblName.FontSize = 18;
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

                        // Label of RequestTimeline
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
            
            // Groups Events after day
            foreach (Event ev in events)
            {
                // Gets day of Event
                string datekey = ev.datetimestart.Date.ToString("dddd, MMMM d, yyyy");

                // Checks if day already exists, if it does not it gets created
                if (!grouped.ContainsKey(datekey))
                {
                    grouped[datekey] = new GroupedEvent(ev.datetimestart);
                }
                // Add Event to their day
                grouped[datekey].events.Add(ev);
                // Sort
                grouped[datekey].events = grouped[datekey].events.OrderBy(sort => sort.datetimestart).ToList();
            }

            return grouped;
        }
    }
}
