using csharp_project.Data;
using csharp_project.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaktionslogik für Calendar.xaml
    /// </summary>
    public partial class Calendar : UserControl
    {
        public DateTime currentmonth { get; private set; } = DateTime.Now;
        public static List<Food> list_f { get; private set; }
        public static List<Drinks> list_d { get; private set; }

        public Calendar()
        {
            InitializeComponent();
            InitLists();
        }

        public void InitLists()
        {
            list_f = loadItems<Food>();
            list_d = loadItems<Drinks>();
        }

        public void SetCalendar()
        {
            l_date.Content = currentmonth.Date.ToString("y");
            int daycolumn;
            int iweek = 0;
            int idays = DateTime.DaysInMonth(currentmonth.Year, currentmonth.Month);

            int temp = (int)FirstDayOfMonth(currentmonth).DayOfWeek; //starts Sunday
            if ( temp == 0)
            {
                daycolumn = 6;
            }
            else
            {
                daycolumn = temp - 1;
            }

            WeekControl weekctrl = new WeekControl();

            MonthSP.Children.Clear();

            for (int i = 0; i < idays; i++)
            {
                //and last weekctrl if full
                if ((daycolumn == 0) && (i > 0))
                {
                    MonthSP.Children.Add(weekctrl);
                    weekctrl = new WeekControl(); //new one for next week
                    iweek++;
                }

                //load DayControls in week rows
                DayControl currentday = new DayControl();
                currentday.DayNumberLabel.Content = (i+1).ToString();
                currentday.Tag = FirstDayOfMonth(currentmonth).AddDays(i);
                if(((DateTime)currentday.Tag).Date == DateTime.Now.Date)
                {
                    currentday.DayLabelRowBorder.Background = Brushes.Green;
                }
                //add Items to daycolumn
                var dayfood = list_f.FindAll(x => x.expiryTime.Value.Date == ((DateTime)currentday.Tag).Date);
                if (dayfood.Count != 0)
                {
                    currentday.l_food_count.Content = $"{dayfood.Count} Food(s)";
                }

                var daydrink = list_d.FindAll(x => x.expiryTime.Value.Date == ((DateTime)currentday.Tag).Date);
                if (daydrink.Count != 0)
                {
                    currentday.l_drinks_count.Content = $"{daydrink.Count} Drink(s)";
                }
                Grid.SetColumn(currentday, daycolumn);
                weekctrl.WeekRowGrid.Children.Add(currentday);

                daycolumn = ++daycolumn % 7;
            }
            MonthSP.Children.Add(weekctrl);
        }

        private List<T> loadItems<T>() where T : Supplies,new()
        {
            var dbhelper = DataManager.getInstance();
            List<T> list;
            list = dbhelper.Get<T>(currentmonth);
            return list;
        }

        public DateTime FirstDayOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        private void b_previous_Click(object sender, MouseButtonEventArgs e)
        {
            currentmonth = currentmonth.AddMonths(-1);
            InitLists();
            SetCalendar();
        }

        private void b_next_Click(object sender, MouseButtonEventArgs e)
        {
            currentmonth = currentmonth.AddMonths(1);
            InitLists();
            SetCalendar();
        }
    }
}
