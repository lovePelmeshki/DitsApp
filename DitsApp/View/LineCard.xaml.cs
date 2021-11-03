using System.Windows;
using System.Linq;
using DitsApp.Model;
using System;

namespace DitsApp.View
{
    /// <summary>
    /// Interaction logic for LineCard.xaml
    /// </summary>
    public partial class LineCard : Window
    {
        public LineCard()
        {
            InitializeComponent();
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
                LinesDataGrid.Items.Refresh();
            }
        }
    }
}
