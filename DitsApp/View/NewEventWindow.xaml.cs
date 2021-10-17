using DitsApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window
    {
        int selectedStationId;
        int selectedLineId;
        public NewEventWindow()
        {
            InitializeComponent();
            using (ditsappdbContext db = new ditsappdbContext())
            {
                //Event Type
                #region ComboBox EventType
                var queryEventTypes = from eventType in db.EventTypes
                                      select new
                                      {
                                          Id = eventType.EventTypeId,
                                          Type = eventType.EventName
                                      };
                ComboBoxEventType.ItemsSource = queryEventTypes.ToList();
                #endregion

                //Maintainer
                #region ComboBox Maintainers
                var queryMaintainers = from maintainer in db.Employees
                                       select new
                                       {
                                           Id = maintainer.EmployeeId,
                                           Lastname = maintainer.Lastname,
                                           Firstname = maintainer.Firstname,
                                           Middlename = maintainer.Middlename
                                       };
                ComboBoxMaintainer.ItemsSource = queryMaintainers.ToList();
                #endregion

                //Lines
                #region Lines
                var queryLines = from line in db.Lines
                                 select new
                                 {
                                     Id = line.LineId,
                                     Name = line.LineName
                                 };
                ComboBoxLine.ItemsSource = queryLines.ToList();


                #endregion
            }
        }

        //Вносить изменения в базу данных при нажатии на кнопку ОК
        private void Button_Click(object sender, RoutedEventArgs e) //Обернуть в блок try catch, сделать валидацию данных
        {

            using (ditsappdbContext db = new ditsappdbContext())
            {
                db.Events.Add(new Event
                {
                    Comment = CommentTextBox.Text,
                    EventTypeId = (int)ComboBoxEventType.SelectedValue,
                    RespoinderId = ComboBoxMaintainer.SelectedValue as int?,
                    StationId = (int)ComboBoxStation.SelectedValue,
                    CreateDate = DateTime.Now,
                    LocationId = ComboBoxPost.SelectedValue as int?,
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

                ComboBoxPost.ItemsSource = null;
                if (ComboBoxStation.SelectedValue != null) selectedStationId = (int)ComboBoxStation.SelectedValue;
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


        private void ComboBoxLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            using (ditsappdbContext db = new ditsappdbContext())
            {
                ComboBoxStation.ItemsSource = null;
                selectedLineId = (int)ComboBoxLine.SelectedValue;
                var queryStations = from line in db.Lines
                                    join station in db.Stations
                                    on line.LineId equals station.LineId
                                    where station.LineId == selectedLineId
                                    select new
                                    {
                                        Id = station.StationId,
                                        Name = station.StationName
                                    };
                ComboBoxStation.ItemsSource = queryStations.ToList();

            }

        }
    }
}
