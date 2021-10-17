using DitsApp.Model;
using DitsApp.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace DitsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int _minRowHeigth;
        public MainWindow()
        {
            InitializeComponent();
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //employee 
                #region Запрос Employees
                var queryAllEmployees = from employee in db.Employees
                                        join department in db.Departments
                                        on employee.DepartmentId equals department.DepartmentId
                                        select new EmployeeInfo()
                                        {
                                            Id = employee.EmployeeId,
                                            Lastname = employee.Lastname,
                                            Firstname = employee.Firstname,
                                            Middlename = employee.Middlename,
                                            Department = department.DepartmentName
                                        };
                EmployeeDataGrid.ItemsSource = queryAllEmployees.ToList();
                #endregion

                //Equipment 
                #region Запрос Equipment

                var queryEquipment = from equipment in db.Equipment
                                     select equipment;
                DataGridEquipment.ItemsSource = queryEquipment.ToList();
                #endregion

                //Events 
                #region Запрос Events
                //Events DataGrid
                //var queryEvents = from e in db.Events
                //                  join eventType in db.EventTypes
                //                  on e.EventTypeId equals eventType.EventTypeId
                //                  join station in db.Stations
                //                  on e.StationId equals station.StationId
                //                  join location in db.Locations
                //                  on e.StationId equals location.StationId
                //                  join line in db.Lines
                //                  on station.LineId equals line.LineId
                //                  orderby e.EventId
                //                  select new
                //                  {
                //                      Id = e.EventId,
                //                      Type = eventType.EventName,
                //                      Line = line.LineName,
                //                      Station = station.StationName,
                //                      Location = location.LocationName
                //                  };

                var queryEvents = from ev in db.Events
                                  join evType in db.EventTypes
                                  on ev.EventTypeId equals evType.EventTypeId
                                  join station in db.Stations
                                  on ev.StationId equals station.StationId

                                  join location in db.Locations
                                  on ev.LocationId equals location.LocationId into ls
                                  from location in ls.DefaultIfEmpty()
                                  select new
                                  {
                                      Id = ev.EventId,
                                      Type = evType.EventName,
                                      Station = station.StationName,
                                      Location = location == null? "---" : location.LocationName
                                  };
                DataGridEvents.ItemsSource = queryEvents.ToList();
                #endregion

                //Maintenances
                #region Запрос Maintenances
                var queryMaintenances = from m in db.Maintenances
                                        select m;
                MaintenancesDataGrid.ItemsSource = queryMaintenances.ToList();
                #endregion

                //Stations
                #region Запрос Stations
                var queryStations = from station in db.Stations
                                    select station;
                DataGridStations.ItemsSource = queryStations.ToList();
                #endregion
            }
        }
        //DoubleClick DataGrid
        private void EmployeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var employee = dataGrid.SelectedItem as EmployeeInfo;
            var infoWindow = new EmployeeInfoWindow(employee);
            infoWindow.Show();


        }

        //Добавить новый Event
        private void NewEventButton_Click(object sender, RoutedEventArgs e)
        {
            var newEventWindow = new NewEventWindow();
            newEventWindow.Show();
        }

        //Открыть окно Add New Equipment
        private void ButtonAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewEquipmentWindow();
            window.Show();
        }

        //Открыть окно Add New Event
        private void AddNewEventButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewEventWindow();
            window.Show();
        }

        //Обновить DataGrid Events DataSource
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновление происходит через повторный запрос к БД
            //ПЕРЕДЕЛАТЬ
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var queryEvents = from ev in db.Events
                                  join evType in db.EventTypes
                                  on ev.EventTypeId equals evType.EventTypeId
                                  join station in db.Stations
                                  on ev.StationId equals station.StationId

                                  join location in db.Locations
                                  on ev.LocationId equals location.LocationId into ls
                                  from location in ls.DefaultIfEmpty()
                                  select new
                                  {
                                      Id = ev.EventId,
                                      Type = evType.EventName,
                                      Station = station.StationName,
                                      Location = location == null ? "---" : location.LocationName
                                  };
                DataGridEvents.ItemsSource = queryEvents.ToList();
            }


        }

        private void DataGridEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var selectedEventId = (int)dataGrid.SelectedValue;
            var window = new EventCard(selectedEventId);
            window.Show();
        }
    }

}


