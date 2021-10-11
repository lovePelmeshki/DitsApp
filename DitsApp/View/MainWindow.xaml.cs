using DitsApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DitsApp.Model;



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
        //DoubleClick DataGrid
        private void EmployeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            var employee = dataGrid.SelectedItem as EmployeeInfo;
            var infoWindow = new EmployeeInfoWindow(employee);
            infoWindow.Show();


        }
        //New Event Button
        private void NewEventButton_Click(object sender, RoutedEventArgs e)
        {
            var newEventWindow = new NewEventWindow();
            newEventWindow.Show();
        }

        private void BottonShowEvents_Click(object sender, RoutedEventArgs e)
        {
            var window = new EventsWindow();
            window.Show();
        }
    }

}


