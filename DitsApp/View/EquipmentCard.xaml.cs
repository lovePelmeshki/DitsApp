using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using DitsApp.Model;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for EquipmentCard.xaml
    /// </summary>
    public partial class EquipmentCard : Window
    {
        public EquipmentCard()
        {
            InitializeComponent();
        }

        public EquipmentCard(int equipmentId)
        {
            InitializeComponent();

            using (ditsappdbContext db = new ditsappdbContext())
            {
                var equip = from e in db.Equipment
                            where e.Id == equipmentId

                            from t in db.EquipmentTypes
                            where e.TypeId == t.Id

                            from s in db.EquipmentStatuses
                            where s.EquipmentId == e.Id

                            from emp in db.Employees
                            where s.MaintainerId == emp.Id



                            select new
                            {
                                Id = e.Id,
                                Serial = e.SerialNumber,
                                Type = t.TypeName,
                                Point = s.PointId,
                                Status = s.Status,
                                ChangeDate = s.ChangeDate,
                                StatusType = s.StatusType,
                                CheckupDate = s.CheckupDate,
                                s.NextCheckupDate,
                                s.MaintenanceDate,
                                s.NextMaintenanceDate,
                                Maintainer = emp.Lastname

                            };

                DataContext = equip.ToList();
            }
        }
    }
}
