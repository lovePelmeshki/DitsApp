using DitsApp.Model;
using System.Linq;
using System.Windows;

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
                                  join location in db.Locations
                                  on e.StationId equals location.StationId
                                  join line in db.Lines
                                  on station.LineId equals line.LineId



                                  select new
                                  {
                                      Id = e.EventId,
                                      Type = eventType.EventName,
                                      Line = line.LineName,
                                      Station = station.StationName,
                                      Location = location.LocationName
                                  };
                DataGridEvents.ItemsSource = queryEvents.ToList();

              
            }
        }
    }
}
