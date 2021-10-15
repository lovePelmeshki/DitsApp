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
        public NewEquipmentWindow()
        {
            InitializeComponent();

            using (ditsappdbContext db = new ditsappdbContext())
            {
                var queryClass = from e_class in db.EquipmentClasses
                                 select e_class;
                ComboBoxClass.ItemsSource = queryClass.ToList();

                //var quertType = 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //забрать данные с форм
            }
        }
    }
}
