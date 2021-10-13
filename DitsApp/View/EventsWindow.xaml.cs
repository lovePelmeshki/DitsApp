using DitsApp.Model;
using System.Windows;
using System.Linq;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for EventsWindow.xaml
    /// </summary>
    public partial class EventsWindow : Window
    {
        public EventsWindow()
        {
            InitializeComponent();
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //НЕ РАБОТАЕТ
                //Переписать LINQ

                var queryEvents = from e in db.Events
                                  join eventType in db.EventTypes
                                  on e.EventTypeId equals eventType.EventTypeId
                                  join station in db.Stations
                                  on e.StationId equals station.StationId

                                  select new
                                  {
                                      Id = e.EventId,
                                      Line = station.Line,
                                      Station = station.StationName,
                                      Type = eventType.EventName,
                                      
                                  };
                DataGridEvents.ItemsSource = queryEvents.ToList();
            }
        }
    }
}
