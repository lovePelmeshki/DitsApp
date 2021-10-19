using System.Windows;
using System.Linq;
using DitsApp.Model;
using System.Windows.Controls;

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
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                db.Equipment.Add(new Equipment
                {
                    Id = TextBoxId.Text,
                    TypeId = (int)ComboBoxType.SelectedValue,
                    PointId = 13,
                    Status = 1,
                    CheckupDate = CheckupDatePicker.DisplayDate,
                    NextCheckupDate = CheckupDatePicker.DisplayDate.AddMonths(_installDuration),
                    InstallDate = InstallDatePicker.DisplayDate                    
                });
                db.SaveChanges();
            }
            Close();
        }

        private void ComboBoxClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedClassId = (int)ComboBoxClass.SelectedValue;
            using (ditsappdbContext db = new ditsappdbContext())
            {
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
            _selectedTypeId = (int)ComboBoxType.SelectedValue;

            using (ditsappdbContext db = new ditsappdbContext())
            {
                var types = from type in db.EquipmentTypes
                            where type.Id == _selectedTypeId
                            select type.InstallDuration;
                _installDuration = (int)types.FirstOrDefault();

            }
        }
    }
}
