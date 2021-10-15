using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DitsApp.Model;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window
    {
        int selectedStationId;
        public NewEventWindow()
        {
            InitializeComponent();
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //Event Type
                var queryEventTypes = from eventType in db.EventTypes
                               select new 
                               {
                                   Id = eventType.EventTypeId,
                                   Type = eventType.EventName
                               };
                ComboBoxEventType.ItemsSource = queryEventTypes.ToList();

                //Maintainer
                var queryMaintainers = from maintainer in db.Employees
                                     select new
                                     {
                                         Id = maintainer.EmployeeId,
                                         Lastname = maintainer.Lastname,
                                         Firstname = maintainer.Firstname,
                                         Middlename = maintainer.Middlename
                                     };
                ComboBoxMaintainer.ItemsSource = queryMaintainers.ToList();

                var queryStations = from station in db.Stations
                                  select new
                                  {
                                      Id = station.StationId,
                                      StationName = station.StationName,
                                      Line = station.Line
                                  };
                ComboBoxStation.ItemsSource = queryStations.ToList();

                

               //заполнить комбобокс для постов, чтобы при выборе станции выпадал список с постами станции
               //Пост - это столбец в таблице LOCATION
               //сделать то же самое для оборудования

            }
        }

        //Вносить изменения в базу данных при нажатии на кнопку ОК
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           using (ditsappdbContext db = new ditsappdbContext())
            {
                db.Events.Add(new Event
                {
                    Comment = CommentTextBox.Text,
                    EventTypeId = ComboBoxEventType.SelectedValue as int?,
                    RespoinderId = ComboBoxMaintainer.SelectedValue as int?,
                    StationId = ComboBoxStation.SelectedValue as int?,
                    CreateDate = DateTime.Now,
                    Status = 1,
                });

                db.SaveChanges();

                this.Close();
            }
        }


        //При выборе станции в ComboBox Station менять ItemSource ComboBox Post, чтобы увидеть список Location 
        //для выбранной станции
        private void ComboBoxStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {
                selectedStationId = (int)ComboBoxStation.SelectedValue;
                var queryLocations = from station in db.Stations
                                     join location in db.Locations
                                     on station.StationId equals location.StationId
                                     where station.StationId == selectedStationId
                                     select new
                                     {
                                         Id = location.LocationId,
                                         Post = location.LocationName
                                     };
                ComboBoxPost.ItemsSource = queryLocations.ToList();
            }
            
        }
    }
}
