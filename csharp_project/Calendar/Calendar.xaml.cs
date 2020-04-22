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
        private DateTime _selectedmonth { get; set; } = DateTime.Now;

        public Calendar()
        {
            InitializeComponent();
        }

        public void SetCalendar()
        {
            l_date.Content = _selectedmonth.Date.ToString("y");
            int daycolumn;
            int iweek = 0;
            int idays = DateTime.DaysInMonth(_selectedmonth.Year, _selectedmonth.Month);

            int temp = (int)FirstDayOfMonth(_selectedmonth).DayOfWeek; //starts Sunday
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
                currentday.Tag = FirstDayOfMonth(_selectedmonth).AddDays(i);
                if(((DateTime)currentday.Tag).Date == DateTime.Now.Date)
                {
                    currentday.DayLabelRowBorder.Background = Brushes.Green;
                }
                Grid.SetColumn(currentday, daycolumn);
                weekctrl.WeekRowGrid.Children.Add(currentday);

                daycolumn = ++daycolumn % 7;
            }
            MonthSP.Children.Add(weekctrl);
        }

        public DateTime FirstDayOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        private void b_previous_Click(object sender, MouseButtonEventArgs e)
        {
            _selectedmonth = _selectedmonth.AddMonths(-1);
            SetCalendar();
        }

        private void b_next_Click(object sender, MouseButtonEventArgs e)
        {
            _selectedmonth = _selectedmonth.AddMonths(1);
            SetCalendar();
        }
    }
}
