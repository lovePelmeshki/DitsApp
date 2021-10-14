﻿using System;
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
                var queryMaintenanceInfo = from maintenance in db.Maintenances
                                      from emp in db.Employees
                                      from eq in db.Equipment
                                      from type in db.EquipmentTypes
                                      from loc in db.Locations
                                      from station in db.Stations

                                      where maintenance.MaintainerId == employee.Id
                                        && eq.TypeId == type.TypeId
                                        && emp.EmployeeId == employee.Id
                                        && eq.EquipmentId == maintenance.EquipmentId
                                        && eq.LocationId == loc.LocationId
                                        && station.StationId == loc.StationId

                                      select new 
                                      {
                                          MaintenanceId = maintenance.MaintenanceId,
                                          Station = station.StationName,
                                          Location = loc.LocationName,
                                          EquipmentID = eq.EquipmentId,
                                          EquipmentType = type.TypeName,
                                          MaintainerLastname = emp.Lastname,
                                          MaintainerFirstname = emp.Firstname,
                                          MaintainerMiddlename = emp.Middlename,
                                          Date = maintenance.MaintenanceDate,
                                          DueDate = maintenance.DueDate,
                                      };

                EmployeeMaintenanceInfoDataGrid.ItemsSource = queryMaintenanceInfo.ToList();
            }
            

            

        }
    }

 


}
