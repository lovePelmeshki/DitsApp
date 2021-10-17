﻿using System.Windows;
using System.Linq;

using DitsApp.Model;
namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for EventCard.xaml
    /// </summary>
    public partial class EventCard : Window
    {
        public EventCard()
        {
            InitializeComponent();
        }

        public EventCard(int EventId)
        {
            InitializeComponent();

            using (ditsappdbContext db = new ditsappdbContext())
            {
                
                var queryEvents = from ev in db.Events
                                  where ev.EventId == EventId
                                  join evType in db.EventTypes
                                  on ev.EventTypeId equals evType.EventTypeId
                                  join station in db.Stations
                                  on ev.StationId equals station.StationId

                                  join location in db.Locations
                                  on ev.LocationId equals location.LocationId into ls
                                  from location in ls.DefaultIfEmpty()

                                  join emp in db.Employees
                                  on ev.RespoinderId equals emp.EmployeeId into rs
                                  from emp in rs.DefaultIfEmpty()
                                 
                                  select new
                                  {
                                      Id = ev.EventId,
                                      Type = evType.EventName,
                                      Station = station.StationName,
                                      Location = location == null ? "---" : location.LocationName,
                                      Status = ev.Status,
                                      CreateDate = ev.CreateDate,
                                      CloseDate = ev.CloseDate,
                                      Respoinder = emp == null? "---" : emp.Lastname,
                                      Comment = ev.Comment
                                  };
                DataContext = queryEvents.ToList();
                                        
            }

        }
    }
}
