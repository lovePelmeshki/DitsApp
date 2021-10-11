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

                //var eventList = from e in db.Events
                //                from eType in db.EquipmentTypes
                //                join type in db.EventTypes
                //                on e.EventTypeId equals type.EventTypeId

                //                join station in db.Stations
                //                on e.StationId equals station.StationId

                //                join responder in db.Employees
                //                on e.RespoinderId equals responder.EmployeeId

                //                join creator in db.Employees
                //                on e.CreatorId equals creator.EmployeeId

                //                join equipmentType in db.EquipmentTypes
                //                on  eType.TypeId equals equipmentType.TypeId
                                
                //                select new
                //                {
                //                    Id = e.EventId,
                //                    Type = type.EventName,
                //                    Station = station.StationName,
                //                    Post = e.Post,
                //                    EquipmentType = equipmentType.TypeName, 
                //                    EqupmentId = e.EquipmentId,
                //                    Status = e.Status,
                //                    CreateDate = e.CreateDate,
                //                    CloseDate = e.CloseDate,
                //                    RespondDate = e.RespondDate,
                //                    Respoinder = responder.Lastname,
                //                    Creator = creator.Lastname,
                //                    Comment = e.Comment
                //                };
                //DataGridEvents.ItemsSource = eventList.ToList();
            }
        }
    }
}
