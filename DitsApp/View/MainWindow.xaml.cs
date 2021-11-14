using DitsApp.Model;
using DitsApp.View;
using DitsApp.View.Cards;
using System;
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
        private int _selectedEmployeeId = -1;
        private int _selectedDepartmentId = -1;
        private int _selectedClassId = -1;
        private int _selectedTypeId = -1;
        private int _installDuration = -1;
        private int _maintenanceDuration = -1;
        private Button _AddEquipmentButton;
        private int _selectedEquipmentId = -1;


        public MainWindow()
        {
            InitializeComponent();
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //employee 
                RefreshEmployeeDataSources();

                //Equipment 
                GetEquipmentInfo(true);



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
                                      Location = location == null ? "---" : location.LocationName
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

        #region Обработка событий Employee

        /// <summary>
        /// Возвращает список Employees
        /// </summary>
        private List<Employee> GetEmployees()
        {
            using (TestDBContext db = new TestDBContext())
            {
                var employees = (from e in db.Employees
                                 select e).ToList();
                return employees;
            }
        }

        private void RefreshEmployeeDataSources()
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //employee 
                #region Запрос Employees
                EmployeeDataGrid.ItemsSource = null;
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

                var departments = from dep in db.Departments
                                  select dep;
                AddDepartmentComboBox.ItemsSource = departments.ToList();
                EditDepartmentComboBox.ItemsSource = departments.ToList();

            }
        }
        private void RefreshForms()
        {
            _selectedDepartmentId = -1;
            _selectedEmployeeId = -1;
            AddLastnameTextBox.Text = "";
            AddNameTextBox.Text = "";
            AddMiddlenameTextBox.Text = "";
            EditLastnameTextBox.Text = "";
            EditMiddlenameTextBox.Text = "";
            EditNameTextBox.Text = "";
            AddDepartmentComboBox.SelectedIndex = -1;
        }

        // Employee DoubleClick 
        private void EmployeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var selectedEmployee = dataGrid.SelectedItem as EmployeeInfo;
            var infoWindow = new EmployeeInfoWindow(selectedEmployee);
            infoWindow.Show();
        }


        // Выбор Employee и отображение в EditGroupBox
        private void EmployeeDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedValue != null)
            {
                _selectedEmployeeId = (int)dataGrid.SelectedValue;
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    var selectedEmployee = (from emp in db.Employees
                                            where emp.Id == _selectedEmployeeId

                                            from dept in db.Departments
                                            where emp.DepartmentId == dept.Id
                                            select new
                                            {
                                                emp.Lastname,
                                                emp.Firstname,
                                                emp.Middlename,
                                                DepartmentId = dept.Id,
                                                DepartmentName = dept.DepartmentName
                                            })
                                            .FirstOrDefault();
                    EditLastnameTextBox.Text = selectedEmployee.Lastname;
                    EditNameTextBox.Text = selectedEmployee.Firstname;
                    EditMiddlenameTextBox.Text = selectedEmployee.Middlename;
                    EditDepartmentComboBox.SelectedValue = selectedEmployee.DepartmentId;
                    _selectedDepartmentId = selectedEmployee.DepartmentId;
                }
            }


        }


        //Добавить нового Employee
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDepartmentId > 0)
            {
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    Employee newEmployee = new Employee()
                    {
                        Lastname = AddLastnameTextBox.Text,
                        Firstname = AddNameTextBox.Text,
                        Middlename = AddMiddlenameTextBox.Text,
                        DepartmentId = _selectedDepartmentId
                    };
                    db.Employees.Add(newEmployee);
                    db.SaveChanges();
                    RefreshForms();
                    RefreshEmployeeDataSources();


                }
            }

        }

        private void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {

            if (_selectedDepartmentId > 0 && _selectedEmployeeId > 0)
            {
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    Employee updateEmployee = (from emp in db.Employees
                                               where emp.Id == _selectedEmployeeId
                                               select emp).FirstOrDefault();

                    updateEmployee.Lastname = EditLastnameTextBox.Text;
                    updateEmployee.Firstname = EditNameTextBox.Text;
                    updateEmployee.Middlename = EditMiddlenameTextBox.Text;
                    updateEmployee.DepartmentId = _selectedDepartmentId;

                    db.SaveChanges();
                    RefreshForms();
                    RefreshEmployeeDataSources();
                }
            }


        }

        //Delete Employee
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                if (_selectedEmployeeId > 0)
                {
                    Employee selectedEmployee = (from emp in db.Employees
                                                 where emp.Id == _selectedEmployeeId
                                                 select emp).FirstOrDefault();
                    db.Employees.Remove(selectedEmployee);
                    db.SaveChanges();
                    RefreshForms();
                    RefreshEmployeeDataSources();
                }
            }
        }

        private void AddDepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddDepartmentComboBox.SelectedValue != null)
            {
                _selectedDepartmentId = (int)AddDepartmentComboBox.SelectedValue;
            }
        }

        private void EditDepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditDepartmentComboBox.SelectedValue != null)
            {
                _selectedDepartmentId = (int)EditDepartmentComboBox.SelectedValue;
            }
        }
        #endregion
        #endregion

        #region Обработчики Event
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

        #endregion


        #region Обработчики Equipment
        //Refresh Equipment
        private void RefreshEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
   

        }
        /// <summary>
        /// Запрос данных об оборудовании
        /// </summary>
        /// <param name="refreshItemsSource" принимает бул></param>
        private void GetEquipmentInfo(bool DoRefreshItemsSource)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var queryEquipment = from e in db.Equipment
                                     join type in db.EquipmentTypes
                                     on e.TypeId equals type.Id

                                     join eqClass in db.EquipmentClasses
                                     on type.ClassId equals eqClass.Id

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
                                         Class = eqClass.ClassName,
                                         ClassId = eqClass.Id,
                                         Type = type.TypeName,
                                         TypeId = type.Id,
                                         Status = s.Status,
                                         Station = station.StationName,
                                         Location = loc.LocationName,
                                         Point = point.PointName,
                                         ChangeDate = s.ChangeDate

                                     };
                DataGridEquipment.ItemsSource = queryEquipment.ToList();
                if (DoRefreshItemsSource)
                {
                    RefreshFormsAndItemsSources();
                }
            }
        }
        private void RefreshFormsAndItemsSources()
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                AddEquipClassComboBox.ItemsSource = GetEquipmentClasses();
                AddEmployeeComboBox.ItemsSource = GetEmployees();

                EditEquipClassComboBox.ItemsSource = GetEquipmentClasses();
                EditEmployeeComboBox.ItemsSource = GetEmployees();
                EditLineComboBox.ItemsSource = GetLines();
            }
        }
        private List<Line> GetLines()
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var line = from l in db.Lines
                           select l;
                return line.ToList();
            }
        }

        /// <summary>
        /// Получить список EquipmentClass
        /// </summary>
        private List<EquipmentClass> GetEquipmentClasses()
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var equipmentClasses = (from c in db.EquipmentClasses
                                        select c).ToList();
                return equipmentClasses;
            }
        }


        private void AddEquipClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddEquipClassComboBox != null)
            {
                _selectedClassId = (int)AddEquipClassComboBox.SelectedValue;
                AddEquipTypeComboBox.ItemsSource = GetEquipmentTypesByClassId(_selectedClassId);

            }

        }

        /// <summary>
        /// Получить список EquipmentType с выбранным Class Id
        /// </summary>
        private List<EquipmentType> GetEquipmentTypesByClassId(int classId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var equipmentTypes = (from t in db.EquipmentTypes
                                      where t.ClassId == classId
                                      select t).ToList();
                return equipmentTypes;
            }
        }
        /// <summary>
        /// Получить Maintenance Diration по EquipmentType Id
        /// </summary>

        private int GetMaintenanceDurationByTypeId(int equipmentTypeId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var duration = (int)(from t in db.EquipmentTypes
                                where t.Id == equipmentTypeId
                                select t.MaintenanceDuration).FirstOrDefault();
                return duration;
            }
        }

        /// <summary>
        /// Получить InstallDuration по EquipmentType Id
        /// </summary>
        private int GetInstallDurationByTypeId(int equipmentTypeId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var duration = (int)(from t in db.EquipmentTypes
                                     where t.Id == equipmentTypeId
                                     select t.InstallDuration).FirstOrDefault();
                return duration;
            }
        }


        private void AddEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                int equipmentId;
                Equipment equipment = new Equipment
                {
                    TypeId = _selectedTypeId,
                    SerialNumber = AddSerialNumTextBox.Text
                };
                db.Equipment.Add(equipment);
                db.SaveChanges();

                equipmentId = equipment.Id;
                DateTime checkupDate = AddCheckupDatePicker.DisplayDate;
                DateTime maintenanceDate = AddInstallDatePicker.DisplayDate;
                int installDuration = GetInstallDurationByTypeId(_selectedTypeId);
                int maintenanceDuration = GetMaintenanceDurationByTypeId(_selectedTypeId);


                EquipmentStatus status = new EquipmentStatus
                {
                    MaintainerId = (int)AddEmployeeComboBox.SelectedValue,
                    EquipmentId = equipmentId,
                    PointId = 13,
                    Status = 1,
                    ChangeDate = DateTime.Now,
                    CheckupDate = checkupDate,
                    NextCheckupDate = checkupDate.AddMonths(installDuration),
                    MaintenanceDate = maintenanceDate,
                    NextMaintenanceDate = maintenanceDate.AddMonths(maintenanceDuration),
                    InstallDate = maintenanceDate,
                    StatusType = "Получение со склада"
                };

                db.EquipmentStatuses.Add(status);
                db.SaveChanges();
                GetEquipmentInfo(false);
            }
        }

        private void AddEquipTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedTypeId = (int)AddEquipTypeComboBox.SelectedValue;
        }



        //Equipment DoubleClick
        private void DataGridEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGrid dataGrid = sender as DataGrid;
            int selectedEquipmentId = (int)dataGrid.SelectedValue;
            var window = new EquipmentCard(selectedEquipmentId);
            window.Show();
        }
        #endregion

        private void Menu_Lines_Click(object sender, RoutedEventArgs e)
        {
            var window = new LineCard();
            window.Show();
        }


        //не доделано
        private void DataGridEquipment_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid equipmentDataGrid = sender as DataGrid;  
            if (equipmentDataGrid != null)
            {
                _selectedEquipmentId = (int)equipmentDataGrid.SelectedValue;
                using (ditsappdbContext db  = new ditsappdbContext())
                {
                    var selectedEquipment = (from eq in db.Equipment
                                    where eq.Id == _selectedEquipmentId
                                    join type in db.EquipmentTypes
                                    on eq.TypeId equals type.Id

                                    join eqClass in db.EquipmentClasses
                                    on type.ClassId equals eqClass.Id

                                    join s in db.EquipmentStatuses
                                    on eq.Id equals s.EquipmentId

                                    join point in db.Points
                                    on s.PointId equals point.Id

                                    join loc in db.Locations
                                    on point.LocationId equals loc.Id

                                    join station in db.Stations
                                    on loc.StationId equals station.Id

                                    join line in db.Lines
                                    on station.LineId equals line.Id

                                    select new
                                    {
                                        Id = eq.Id,
                                        Serial = eq.SerialNumber,
                                        Class = eqClass.ClassName,
                                        ClassId = eqClass.Id,
                                        Type = type.TypeName,
                                        TypeId = type.Id,
                                        Status = s.Status,
                                        LineId = line.Id,
                                        Line = line.LineName,
                                        StationId = station.Id,
                                        Station = station.StationName,
                                        LocationId = loc.Id,
                                        Location = loc.LocationName,
                                        PointId = point.Id,
                                        Point = point.PointName,
                                        ChangeDate = s.ChangeDate,
                                        CheckupDate = s.CheckupDate,
                                        InstallDate = s.InstallDate,
                                        MaintainerId = s.MaintainerId,

                                    }).SingleOrDefault();

                    EditEquipClassComboBox.SelectedValue = selectedEquipment.ClassId;
                    EditEquipTypeComboBox.SelectedValue = selectedEquipment.TypeId;
                    EditSerialNumTextBox.Text = selectedEquipment.Serial;
                    EditLineComboBox.SelectedValue = selectedEquipment.LineId;
                    EditStationComboBox.SelectedValue = selectedEquipment.StationId;
                    EditLocationComboBox.SelectedValue = selectedEquipment.LocationId;
                    EditPointComboBox.SelectedValue = selectedEquipment.PointId;
                    EditCheckupDatePicker.SelectedDate = selectedEquipment.CheckupDate;
                    EditInstallDatePicker.SelectedDate = selectedEquipment.InstallDate;
                    EditEmployeeComboBox.SelectedValue = selectedEquipment.MaintainerId;
                }

            }
        }

        private void EditEquipClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditEquipClassComboBox.SelectedValue != null)
            {
                _selectedClassId = (int)EditEquipClassComboBox.SelectedValue;
                EditEquipTypeComboBox.ItemsSource = GetEquipmentTypesByClassId(_selectedClassId);
            }
        }

        private List<Station> GetStationsByLineId(int lineId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var station = from s in db.Stations
                           where s.LineId == lineId
                           select s;
                return station.ToList();
            }
        }
    }



}


