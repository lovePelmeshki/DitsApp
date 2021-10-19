using System.Windows;
using System.Linq;
using DitsApp.Model;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for NewEquipmentWindow.xaml
    /// </summary>
    public partial class NewEquipmentWindow : Window
    {
        public int selectedClassId;
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
                    TypeId = (int)ComboBoxType.SelectedValue
                });
                db.SaveChanges();
            }
            Close();
        }

        private void ComboBoxClass_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedClassId = (int)ComboBoxClass.SelectedValue;
            using (ditsappdbContext db = new ditsappdbContext())
            {
                var types = from t in db.EquipmentTypes
                            where t.ClassId == selectedClassId
                            select new
                            {
                                Id = t.Id,
                                Name = t.TypeName
                            };
                ComboBoxType.ItemsSource = types.ToList();
            };
            
            
        }
    }
}
