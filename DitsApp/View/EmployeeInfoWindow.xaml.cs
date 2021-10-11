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
                var maintenanceInfo = from maintenance in db.Maintenances
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
                                          Location = loc.Point,
                                          EquipmentID = eq.EquipmentId,
                                          EquipmentType = type.TypeName,
                                          Post = loc.Post,
                                          MaintainerLastname = emp.Lastname,
                                          MaintainerFirstname = emp.Firstname,
                                          MaintainerMiddlename = emp.Middlename,
                                          Date = maintenance.MaintenanceDate,
                                          DueDate = maintenance.DueDate,
                                          

                                      };

                EmployeeInfoDataGrid.ItemsSource = maintenanceInfo.ToList();
            }
            

            

        }
    }

    public class MaintenanceInfo
    {
        public int ID { get; set; }
        public string Station { get; set; }
        public string Location { get; set; }
        public string EquipmentId { get; set; }
        public string EquipmentType { get; set; }
        public string Post { get; set; }
        public string MaintainerLastName { get; set; }
        public string MaintainerFirstName { get; set; }
        public string MaintainerMiddleName { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
    }


}
