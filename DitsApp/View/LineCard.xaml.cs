using System.Windows;
using System.Linq;
using DitsApp.Model;
using System;
using System.Windows.Controls;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for LineCard.xaml
    /// </summary>
    
    public partial class LineCard : Window
    {
        
        private int _selectedLineId = -1;
        private Button _deleteButton;
        public LineCard()
        {
            InitializeComponent();

            _deleteButton = DeleteButton;
            _deleteButton.IsEnabled = false;

            using (ditsappdbContext db = new ditsappdbContext())
            {
                var lines = from line in db.Lines
                            select line;
                LinesDataGrid.ItemsSource = lines.ToList();
            }
            

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (ditsappdbContext db = new ditsappdbContext())
            {

                db.Lines.Add(new Line
                {
                    Id = int.Parse(LineNumTextBox.Text),
                    LineName = LineNameTextBox.Text
                });
                db.SaveChanges();

                //обновление ItemsSourse
                LinesDataGrid.ItemsSource = null;
                var lines = from line in db.Lines
                            select line;
                LinesDataGrid.ItemsSource = lines.ToList();

                //обнуление текстБоксов
                LineNumTextBox.Text = "";
                LineNameTextBox.Text = "";

            }
        }

        private void LinesDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
            DataGrid datagrid = sender as DataGrid;

            if (datagrid.SelectedValue != null) 
                _selectedLineId = (int)datagrid.SelectedValue; //тут ошибка NullReferenceException после удаления из БД
            _deleteButton.IsEnabled = _selectedLineId < 0 ? false:true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLineId >0)
            {
                using (ditsappdbContext db = new ditsappdbContext())
                {
                    Line selectedLine = (from line in db.Lines
                                         where line.Id == _selectedLineId
                                         select line).FirstOrDefault();

                    if (selectedLine != null)
                    {
                        db.Lines.Remove(selectedLine);
                        _selectedLineId = -1;
                        db.SaveChanges();
                    }
                    LinesDataGrid.ItemsSource = null;
                    var lines = from line in db.Lines
                                select line;
                    LinesDataGrid.ItemsSource = lines.ToList();
                }
            }
        }
    }
}
