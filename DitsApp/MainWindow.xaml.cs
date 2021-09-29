using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DitsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int _minRowHeigth;
        public MainWindow()
        {
            InitializeComponent();
            using (ditsappdbContext db = new ditsappdbContext())
            {

                var employees = from employee in db.Employees
                                join department in db.Departments
                                on employee.DepartmentId equals department.DepartmentId
                                select new EmployeeInfo()
                                {
                                    Id = employee.EmployeeId,
                                    Lastname = employee.Lastname,
                                    Firstname = employee.Firstname,
                                    Middlename = employee.Middlename,
                                    Department = department.DepartmentName
                                };


                EmployeeDataGrid.ItemsSource = employees.ToList();
            }
        }

        private void EmployeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var a = dataGrid.SelectedItem as EmployeeInfo;
            var infoWindow = new EmployeeInfoWindow(a);
            infoWindow.Show();


        }
    }

    public class EmployeeInfo
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Department { get; set; }

    }
}


