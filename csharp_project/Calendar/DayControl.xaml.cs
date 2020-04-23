using csharp_project.Data;
using csharp_project.Views;
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

namespace csharp_project.Calendar
{
    /// <summary>
    /// Interaktionslogik für DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        public DayControl()
        {
            InitializeComponent();
        }

        private void l_food_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dayitems = Calendar.list_f.FindAll(x => x.expiryTime.Value.Date == ((DateTime)Tag).Date);
            CalendarItemsDialog dia = new CalendarItemsDialog(dayitems, "Food");
            dia.ShowDialog();
        }

        private void l_drinks_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dayitems = Calendar.list_d.FindAll(x => x.expiryTime.Value.Date == ((DateTime)Tag).Date);
            CalendarItemsDialog dia = new CalendarItemsDialog(dayitems, "Drinks");
            dia.ShowDialog();
        }
    }
}
