using DitsApp.Model;
using DitsApp.View;
using DitsApp.View.Cards;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#region scaffold
/*
Scaffold-DbContext "Server=tcp:ditsapp.database.windows.net,1433;Initial Catalog=ditsappdb;Persist Security Info=False;User ID=lovepelmeshki;Password=90f8b7rr#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model -Force
*/

#endregion

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
                                        on employee.DepartmentId equals department.Id
                                        select new EmployeeInfo()
                                        {
                                            Id = employee.Id,
                                            Lastname = employee.Lastname,
                                            Firstname = employee.Firstname,
                                            Middlename = employee.Middlename,
                                            Department = department.DepartmentName
                                        };
                EmployeeDataGrid.ItemsSource = queryAllEmployees.ToList();
                #endregion

                //Equipment 
                #region Запрос Equipment

                var queryEquipment = from e in db.Equipment
                                     join type in db.EquipmentTypes
                                     on e.TypeId equals type.Id

                                     join s in db.EquipmentStatuses 
                                     on e.Id equals s.EquipmentId

                                     join point in db.Points
                                     on s.PointId equals point.Id

                                     join loc in db.Locations
                                     on point.LocationId equals loc.Id

                                     join station in db.Stations
                                     on loc.StationId equals station.Id

                                     select new
                                     {
                                         Id = e.Id,
                                         Serial = e.SerialNumber,
                                         Type = type.TypeName,
                                         Status = s.Status ,
                                         Station = station.StationName,
                                         Location = loc.LocationName,
                                         Point = point.PointName,
                                         
                                     };
                DataGridEquipment.ItemsSource = queryEquipment.ToList();

                #endregion

                //Events 
                #region Запрос Events
                //Events DataGrid

                var queryEvents = from ev in db.Events
                                  join evType in db.EventTypes
                                  on ev.TypeId equals evType.Id
                                  join station in db.Stations
                                  on ev.StationId equals station.Id

                                  join location in db.Locations
                                  on ev.LocationId equals location.Id into ls
                                  from location in ls.DefaultIfEmpty()
                                  select new
                                  {
                                      Id = ev.Id,
                                      Type = evType.TypeName,
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
        // Employee DoubleClick 
        private void EmployeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var selectedEmployee = dataGrid.SelectedItem as EmployeeInfo;
            var infoWindow = new EmployeeInfoWindow(selectedEmployee);
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

        //Refresh Events
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновление происходит через повторный запрос к БД
            //ПЕРЕДЕЛАТЬ
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var queryEvents = from ev in db.Events
                                  join evType in db.EventTypes
                                  on ev.TypeId equals evType.Id
                                  join station in db.Stations
                                  on ev.StationId equals station.Id

                                  join location in db.Locations
                                  on ev.LocationId equals location.Id into ls
                                  from location in ls.DefaultIfEmpty()
                                  select new
                                  {
                                      Id = ev.Id,
                                      Type = evType.TypeName,
                                      Station = station.StationName,
                                      Location = location == null ? "---" : location.LocationName
                                  };
                DataGridEvents.ItemsSource = queryEvents.ToList();
            }


        }


        //Events DoubleClick
        private void DataGridEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            int selectedEventId = (int)dataGrid.SelectedValue;
            var window = new EventCard(selectedEventId);
            window.Show();
        }


        //Refresh Equipment
        private void RefreshEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var queryEquipment = from eq in db.Equipment
                                     join type in db.EquipmentTypes
                                     on eq.TypeId equals type.Id

                                     join s in db.EquipmentStatuses
                                     on eq.Id equals s.EquipmentId

                                     join point in db.Points
                                     on s.PointId equals point.Id

                                     join loc in db.Locations
                                     on point.LocationId equals loc.Id

                                     join station in db.Stations
                                     on loc.StationId equals station.Id

                                     select new
                                     {
                                         Id = eq.Id,
                                         Serial = eq.SerialNumber,
                                         Type = type.TypeName,
                                         Status = s.Status,
                                         Station = station.StationName,
                                         Location = loc.LocationName,
                                         Point = point.PointName,

                                     };
                DataGridEquipment.ItemsSource = queryEquipment.ToList();
            }

        }

        //Equipment DoubleClick
        private void DataGridEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGrid dataGrid = sender as DataGrid;
            int selectedEquipmentId = (int)dataGrid.SelectedValue;
            var window = new EquipmentCard(selectedEquipmentId);
            window.Show();
        }

        private void Menu_Lines_Click(object sender, RoutedEventArgs e)
        {
            var window = new LineCard();
            window.Show();
        }
    }

}


