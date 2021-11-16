using DitsApp.Model;
using DitsApp.View;
using DitsApp.View.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Point = DitsApp.Model.Point;

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
        private int _selectedEquipmentId = -1;
        private int _selectedEventId = -1;
        private int _selectedLineId_Event = -1;
        private int _selectedStationId_Event = -1;
        private int _selectedLocationId_Event = -1;



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
                GetEventsInfo(true);
                GetClosedEventsInfo();

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

        private void GetEventsInfo(bool doRefreshItemSource)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var events = from ev in db.Events
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
                DataGridEvents.ItemsSource = events.ToList();
                if (doRefreshItemSource)
                {
                    SetEventsDataSource();
                }
            }
        }


        private void GetClosedEventsInfo()
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var events = from ev in db.Events
                             where ev.Status == 0
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
                DataGridClosedEvents.ItemsSource = events.ToList();
            }
        }
        private Event GetEvent(int id)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var _event = (from ev in db.Events
                              where ev.Id == id
                              select ev).SingleOrDefault();
                return _event;
            }
        }
        private List<EventType> GetEventTypes()
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var types = from t in db.EventTypes
                            select t;
                return types.ToList();
            }
        }
        private void SetEventsDataSource()
        {

            ComboBoxEventType.ItemsSource = GetEventTypes();
            ComboBoxLineEvent.ItemsSource = GetLines();
        }


        #endregion


        #region Обработчики Equipment
        //Refresh Equipment

        private void GetEquipmentInfo(bool DoRefreshItemsSource)
        {
            DataGridEquipment.ItemsSource = null;
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var queryEquipment = from e in db.Equipment
                                     join type in db.EquipmentTypes
                                     on e.TypeId equals type.Id

                                     join eqClass in db.EquipmentClasses
                                     on type.ClassId equals eqClass.Id

                                     join s in db.EquipmentStatuses
                                     on e.Id equals s.EquipmentId

                                     join emp in db.Employees
                                     on s.MaintainerId equals emp.Id

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
                                         ChangeDate = s.ChangeDate,
                                         MaintainerId = emp.Id,
                                         LastChange = s.StatusType


                                     };
                DataGridEquipment.ItemsSource = queryEquipment.ToList();
                if (DoRefreshItemsSource)
                {
                    RefreshItemsSources();
                }
            }
        }
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
        private List<EquipmentClass> GetEquipmentClasses()
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var equipmentClasses = (from c in db.EquipmentClasses
                                        select c).ToList();
                return equipmentClasses;
            }
        }
        private Equipment GetEquipment(int equipmentId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                Equipment equipment = (from eqip in db.Equipment
                                       where eqip.Id == equipmentId
                                       select eqip).SingleOrDefault();
                return equipment;
            }
        }
        private EquipmentStatus GetCurrentStatus(Equipment equipment)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                EquipmentStatus status = (from s in db.EquipmentStatuses
                                          where s.EquipmentId == equipment.Id && s.Status == 1
                                          select s).SingleOrDefault();
                return status;

            }
        }
        private void SetEquipmentStatus(Equipment equipment, EquipmentStatus newEquipmentStatus)
        {
            EquipmentStatus currentStatus = GetCurrentStatus(equipment);
            currentStatus.Status = 0;
        }
        private List<EquipmentStatus> GetStatusList(Equipment equipment)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var statusList = from s in db.EquipmentStatuses
                                 where s.EquipmentId == equipment.Id
                                 select s;
                return statusList.ToList();


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
        private List<Station> GetStationsList(int lineId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var station = from s in db.Stations
                              where s.LineId == lineId
                              select s;
                return station.ToList();
            }
        }
        private List<Location> GetLocationList(int stationId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var locations = from l in db.Locations
                                where l.StationId == stationId
                                select l;
                return locations.ToList();
            }
        }
        private List<Point> GetPointList(int locationId)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var points = from p in db.Points
                             where p.LocationId == locationId
                             select p;
                return points.ToList();
            }
        }
        private void RefreshItemsSources()
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
        private void RefreshEquipmentForms()
        {
            EditEquipClassComboBox.SelectedIndex = -1;
            EditEquipTypeComboBox.SelectedIndex = -1;
            EditSerialNumTextBox.Text = "";
            EditLineComboBox.SelectedIndex = -1;
            EditStationComboBox.SelectedIndex = -1;
            EditLocationComboBox.SelectedIndex = -1;
            EditPointComboBox.SelectedIndex = -1;
            EditCheckupDatePicker.SelectedDate = null;
            EditInstallDatePicker.SelectedDate = null;
            EditEmployeeComboBox.SelectedIndex = -1;
        }
        private void AddEquipClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddEquipClassComboBox != null)
            {
                _selectedClassId = (int)AddEquipClassComboBox.SelectedValue;
                AddEquipTypeComboBox.ItemsSource = GetEquipmentTypesByClassId(_selectedClassId);

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
                    StatusType = "Получение со склада",
                };

                db.EquipmentStatuses.Add(status);
                db.SaveChanges();
                GetEquipmentInfo(false);
                RefreshEquipmentForms();
            }
        }
        private void AddEquipTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedTypeId = (int)AddEquipTypeComboBox.SelectedValue;
        }
        private void DataGridEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGrid dataGrid = sender as DataGrid;
            int selectedEquipmentId = (int)dataGrid.SelectedValue;
            var window = new EquipmentCard(selectedEquipmentId);
            window.Show();
        }
        //selection changed
        private void DataGridEquipment_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid equipmentDataGrid = sender as DataGrid;
            if (equipmentDataGrid != null && equipmentDataGrid.SelectedValue != null)
            {
                _selectedEquipmentId = (int)equipmentDataGrid.SelectedValue;
                using (ditsappdbContext db = new ditsappdbContext())
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

                                             join emp in db.Employees
                                             on s.MaintainerId equals emp.Id


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
                                                 Lastname = emp.Lastname

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
        private void EditLineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditLineComboBox.SelectedValue != null)
            {
                EditStationComboBox.ItemsSource = GetStationsList((int)EditLineComboBox.SelectedValue);
            }
        }
        private void EditStationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditStationComboBox.SelectedValue != null)
            {
                EditLocationComboBox.ItemsSource = GetLocationList((int)EditStationComboBox.SelectedValue);
            }
        }
        private void EditLocationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditLocationComboBox.SelectedValue != null)
            {
                EditPointComboBox.ItemsSource = GetPointList((int)EditLocationComboBox.SelectedValue);
            }
        }
        private void DeleteEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEquipmentId > 0)
            {
                Equipment selectedEquipment = GetEquipment(_selectedEquipmentId);
                EquipmentStatus status = GetCurrentStatus(selectedEquipment);
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    db.EquipmentStatuses.Remove(status);
                    db.SaveChanges();
                    db.Equipment.Remove(selectedEquipment);
                    db.SaveChanges();
                    _selectedEquipmentId = -1;
                    GetEquipmentInfo(true);

                }

            }

        }
        private void EditEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEquipmentId > 0)
            {
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    Equipment selectedEquipment = GetEquipment(_selectedEquipmentId);
                    EquipmentStatus currentStatus = GetCurrentStatus(selectedEquipment);

                    //selectedEquipment.TypeId = (int)EditEquipTypeComboBox.SelectedValue;
                    //selectedEquipment.SerialNumber = EditSerialNumTextBox.Text;
                    //db.Equipment.Update(selectedEquipment);
                    //db.SaveChanges();

                    currentStatus.PointId = (int)EditPointComboBox.SelectedValue;
                    currentStatus.ChangeDate = DateTime.Now;
                    currentStatus.CheckupDate = EditCheckupDatePicker.DisplayDate;
                    currentStatus.InstallDate = EditInstallDatePicker.DisplayDate;
                    currentStatus.MaintainerId = (int)EditEmployeeComboBox.SelectedValue;
                    currentStatus.StatusType = "Установка";
                    db.EquipmentStatuses.Update(currentStatus);
                    db.SaveChanges();

                    GetEquipmentInfo(true);
                    RefreshEquipmentForms();
                }
            }
        }
        private void ButtonGoToRepair_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEquipmentId > 0)
            {
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    Equipment selectedEquipment = GetEquipment(_selectedEquipmentId);
                    EquipmentStatus currentStatus = GetCurrentStatus(selectedEquipment);

                    currentStatus.PointId = 14;
                    currentStatus.ChangeDate = DateTime.Now;
                    currentStatus.StatusType = "Сдача в ремонт";
                    db.EquipmentStatuses.Update(currentStatus);
                    db.SaveChanges();
                    GetEquipmentInfo(true);
                    RefreshEquipmentForms();

                }
            }

        }

        #endregion

        private void Menu_Lines_Click(object sender, RoutedEventArgs e)
        {
            var window = new LineCard();
            window.Show();
        }



        private void DataGridEvents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.SelectedValue != null)
            {
                _selectedEventId = (int)dataGrid.SelectedValue;

            }
        }

        private void ComboBoxLineEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxLineEvent.SelectedValue != null)
            {
                _selectedLineId_Event = (int)ComboBoxLineEvent.SelectedValue;
                ComboBoxStationEvent.ItemsSource = GetStationsList(_selectedLineId_Event);

            }
        }
        private void ComboBoxStationEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxStationEvent.SelectedValue != null && _selectedLineId_Event > 0)
            {
                _selectedStationId_Event = (int)ComboBoxStationEvent.SelectedValue;
                ComboBoxLocationEvent.ItemsSource = GetLocationList(_selectedStationId_Event);
            }

        }

        private void ComboBoxLocationEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxLocationEvent.SelectedValue != null && _selectedStationId_Event > 0)
            {
                _selectedLocationId_Event = (int)ComboBoxLocationEvent.SelectedValue;
                ComboBoxPointEvent.ItemsSource = GetPointList(_selectedLocationId_Event);
            }
        }

        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                db.Events.Add(new Event
                {
                    TypeId = (int)ComboBoxEventType.SelectedValue,
                    StationId = (int)ComboBoxStationEvent.SelectedValue,
                    LocationId = (int)ComboBoxLocationEvent.SelectedValue,
                    Status = 1,
                    CreateDate = DatePickerEvent.DisplayDate,
                    Comment = CommentTextBoxEvent.Text
                }); 
                db.SaveChanges();
                GetEventsInfo(true);
                
            }

        }

        private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEventId > 0)
            {
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    Event selectedEvent = GetEvent(_selectedEventId);
                    db.Events.Remove(selectedEvent);
                    db.SaveChanges();
                    GetEventsInfo(true);
                }
            }
        }
    }
}


