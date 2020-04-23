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
        public Calendar parent { get; private set; }
        public DayControl()
        {
            InitializeComponent();
        }
        public DayControl( Calendar source)
        {
            InitializeComponent();
            parent = source;
        }

        private void l_food_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalendarItemsDialog dia = new CalendarItemsDialog("Food", this);
            dia.ShowDialog();
        }

        private void l_drinks_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalendarItemsDialog dia = new CalendarItemsDialog("Drinks", this);
            dia.ShowDialog();
        }
    }
}
