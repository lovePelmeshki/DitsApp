using System.Windows;
using System.Linq;
using DitsApp.Model;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for EquipmentWindow.xaml
    /// </summary>
    public partial class EquipmentWindow : Window
    {
        public EquipmentWindow()
        {
            InitializeComponent();

            using (ditsappdbContext db = new ditsappdbContext())
            {
                var queryEquipment = from equipment in db.Equipment
                                     select equipment;
                DataGridEquipment.ItemsSource = queryEquipment.ToList();

            }
        }

        private void ButtonAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewEquipmentWindow();
            window.Show();
        }
    }
}
