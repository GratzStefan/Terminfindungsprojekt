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
        private List<PostEvent> events = new List<PostEvent>();
        public HomepageControl()
        {
            InitializeComponent();
            DisplayEvents();
        }

        private async void DisplayEvents()
        {
            staEvents.Children.Clear();
            events = await APICall.RunAsync<List<PostEvent>>($"http://localhost:8080/api/events/find/{User.GetInstance(null).ID}", null);

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
            foreach (PostEvent ev in events)
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
    }
}
