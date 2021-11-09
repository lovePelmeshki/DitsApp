using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using DitsApp.Model;

namespace DitsApp.View.Cards
{
    /// <summary>
    /// Interaction logic for EquipmentCard.xaml
    /// </summary>
    public partial class EquipmentCard : Window
    {

        public EquipmentCard(int equipmentId)
        {
            InitializeComponent();


            //запрос выбрать оборудование с equipmentId
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var equip = from equipment in db.Equipment
                            where equipment.Id == equipmentId

                            from t in db.EquipmentTypes
                            where equipment.TypeId == t.Id

                            from status in db.EquipmentStatuses
                            where status.EquipmentId == equipment.Id

                            from point in db.Points
                            where status.PointId == point.Id
                            from location in db.Locations
                            where point.LocationId == location.Id
                            from station in db.Stations
                            where location.StationId == station.Id
                            from line in db.Lines
                            where station.LineId == line.Id

                            from emp in db.Employees
                            where status.MaintainerId == emp.Id

                            select new
                            {
                                Id = equipment.Id,
                                Serial = equipment.SerialNumber,
                                Type = t.TypeName,
                                Line = line.LineName,
                                Station = station.StationName,
                                Location = location.LocationName,
                                Point = point.PointName,
                                Status = status.Status,
                                ChangeDate = status.ChangeDate,
                                StatusType = status.StatusType,
                                CheckupDate = status.CheckupDate,
                                NextCheckupDate = status.NextCheckupDate,
                                MaintenanceDate = status.MaintenanceDate,
                                NextMaintenanceDate = status.NextMaintenanceDate,
                                Maintainer = emp.Lastname

                            };

                DataContext = equip.ToList();
            }
        }
    }
}
