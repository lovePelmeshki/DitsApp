using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using DitsApp.Model;
using DitsApp.View;

namespace DitsApp
{
    /// <summary>
    /// Interaction logic for EmployeeInfoWindow.xaml
    /// </summary>
    public partial class EmployeeInfoWindow : Window
    {
        public EmployeeInfoWindow()
        {
            InitializeComponent();
        }
        public EmployeeInfoWindow(EmployeeInfo employee)
        {
            InitializeComponent();
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //var queryMaintenanceInfo = from maintenance in db.Maintenances
                //                      from emp in db.Employees
                //                      from eq in db.Equipment
                //                      from type in db.EquipmentTypes
                //                      from point in db.Points
                //                      from loc in db.Locations
                //                      from station in db.Stations

                //                      where maintenance.MainteinerId == employee.Id
                //                        && eq.TypeId == type.Id
                //                        && emp.Id == employee.Id
                //                        && eq.Id == maintenance.EquipmentId
                //                        && eq.PointId == point.Id
                //                        && point.LocationId == loc.Id
                //                        && station.Id == loc.StationId

                //                      select new 
                //                      {
                //                          MaintenanceId = maintenance.Id,
                //                          Station = station.StationName,
                //                          Point = point.PointName,
                //                          Location = loc.LocationName,
                //                          EquipmentID = eq.Id,
                //                          EquipmentType = type.TypeName,
                //                          MaintainerLastname = emp.Lastname,
                //                          MaintainerFirstname = emp.Firstname,
                //                          MaintainerMiddlename = emp.Middlename,
                //                          Date = maintenance.MaintenanceDate,
                //                          DueDate = maintenance.DurationDate,
                //                      };

                //EmployeeMaintenanceInfoDataGrid.ItemsSource = queryMaintenanceInfo.ToList();

                var queryEventsInfo = from ev in db.Events
                                      join evType in db.EventTypes
                                      on ev.TypeId equals evType.Id
                                      join station in db.Stations
                                      on ev.StationId equals station.Id

                                      join location in db.Locations
                                      on ev.LocationId equals location.Id into ls
                                      from location in ls.DefaultIfEmpty()
                                      where ev.RespoinderId == employee.Id
                                      select new
                                      {
                                          Id = ev.Id,
                                          Type = evType.TypeName,
                                          Station = station.StationName,
                                          Location = location == null ? "---" : location.LocationName
                                      };
                DataGridEmployeeEventInfo.ItemsSource = queryEventsInfo.ToList();
            }
            

            

        }

        private void DataGridEmployeeEventInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var selectedEventId = (int)dataGrid.SelectedValue;
            var window = new EventCard(selectedEventId);
            window.Show();
        }
    }

 


}
