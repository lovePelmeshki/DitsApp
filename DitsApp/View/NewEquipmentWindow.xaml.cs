using System.Windows;
using System.Linq;
using DitsApp.Model;
using System.Windows.Controls;
using System;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for NewEquipmentWindow.xaml
    /// </summary>
    public partial class NewEquipmentWindow : Window
    {
        private int _selectedClassId;
        private int _selectedTypeId;
        private int _installDuration;
        private int _maintenanceDuration;
        public NewEquipmentWindow()
        {
            InitializeComponent();

            using (ditsappdbContext db = new ditsappdbContext())
            {
                var classes = from c in db.EquipmentClasses
                              select new
                              {
                                  Id = c.Id,
                                  Name = c.ClassName
                              };
                ComboBoxClass.ItemsSource = classes.ToList();

                var employees = from e in db.Employees
                                select new
                                {
                                    Id = e.Id,
                                    Name = e.Lastname
                                };
                ComboBoxEmployee.ItemsSource = employees.ToList();
            }
        }

        //Add new Equip
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                string serialNumber = TextBoxId.Text;
                DateTime checkupDate = CheckupDatePicker.DisplayDate;
                DateTime maintenanceDate = InstallDatePicker.DisplayDate;

                db.Equipment.Add(new Equipment
                {
                    SerialNumber = serialNumber,
                    TypeId = (int)ComboBoxType.SelectedValue
                                       
                });

                db.SaveChanges();

                var newEquipmentId = from eq in db.Equipment
                                     where eq.SerialNumber == serialNumber &&
                                     eq.TypeId == _selectedTypeId
                                     select eq.Id;
                int eqId = newEquipmentId.FirstOrDefault();

                   db.EquipmentStatuses.Add(new EquipmentStatus
                {
                    EquipmentId = eqId,
                    PointId = 13,
                    Status = 1,
                    ChangeDate = DateTime.Now,
                    CheckupDate = checkupDate,
                    NextCheckupDate = checkupDate.AddMonths(_installDuration),
                    MaintenanceDate = maintenanceDate,
                    NextMaintenanceDate = maintenanceDate.AddMonths(_maintenanceDuration),
                    InstallDate = maintenanceDate,
                    MaintainerId = (int)ComboBoxEmployee.SelectedValue,
                    StatusType = "Получение со склада"

                });
                db.SaveChanges();
            }
            Close();
        }

        private void ComboBoxClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxType.ItemsSource = null;
            using (ditsappdbContext db = new ditsappdbContext())
            {
 
                if (ComboBoxClass.SelectedValue != null) _selectedClassId = (int)ComboBoxClass.SelectedValue;

                var types = from t in db.EquipmentTypes
                            where t.ClassId == _selectedClassId
                            select new
                            {
                                Id = t.Id,
                                Name = t.TypeName,
                                _installDuration = t.InstallDuration
                            };
                ComboBoxType.ItemsSource = types.ToList();
            };
              
        }

        private void ComboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            using (ditsappdbContext db = new ditsappdbContext())
            {

                if (ComboBoxType.SelectedValue != null) _selectedTypeId = (int)ComboBoxType.SelectedValue;

                var installDuration = from type in db.EquipmentTypes
                            where type.Id == _selectedTypeId
                            select type.InstallDuration;

                var maintenanceDuration = from type in db.EquipmentTypes
                                          where type.Id == _selectedTypeId
                                          select type.MaintenanceDuration;

                _installDuration = (int)installDuration.FirstOrDefault();
                _maintenanceDuration = (int)maintenanceDuration.FirstOrDefault();


            }
        }
    }
}
