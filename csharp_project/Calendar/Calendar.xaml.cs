using csharp_project.Data;
using csharp_project.DataAccess;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace csharp_project.Calendar
{
    /// <summary>
    /// Interaktionslogik für Calendar.xaml
    /// </summary>
    public partial class Calendar : UserControl
    {
        #region Private Fields

        private readonly UnityContainer _container;

        #endregion Private Fields

        #region Public Constructors

        public Calendar(UnityContainer container)
        {
            InitializeComponent();

            _container = container;
        }

        #endregion Public Constructors

        #region Public Properties

        public List<Drinks> List_d { get; private set; }

        public List<Food> List_f { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private DateTime _currentmonth = DateTime.Now;

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Helper function to get first day of given month
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTime FirstDayOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        /// <summary>
        /// Loads Items for given month
        /// </summary>
        public void InitLists()
        {
            List_f = loadItems<Food>();
            List_d = loadItems<Drinks>();
        }

        /// <summary>
        /// CalendarLogic
        /// calculates dayofWeek from currentmonth for daycolumns
        /// Fills weekcontrols with DayControls
        /// Adds weeecontrols to calendar stackpanel
        /// </summary>
        public void SetCalendar()
        {
            l_date.Content = _currentmonth.Date.ToString("y");

            int daycolumn;
            int iweek = 0;
            int idays = DateTime.DaysInMonth(_currentmonth.Year, _currentmonth.Month);

            int temp = (int)FirstDayOfMonth(_currentmonth).DayOfWeek; //starts Sunday

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
                DayControl currentday = new DayControl(this, _container);
                currentday.DayNumberLabel.Content = (i+1).ToString();
                currentday.Tag = FirstDayOfMonth(_currentmonth).AddDays(i);

                if(((DateTime)currentday.Tag).Date == DateTime.Now.Date)
                {
                    currentday.DayLabelRowBorder.Background = Brushes.Green;
                }

                //add Items to daycolumn
                var dayfood = List_f.FindAll(x => x.ExpiryTime.Value.Date == ((DateTime)currentday.Tag).Date);

                if (dayfood.Count != 0)
                {
                    currentday.l_food_count.Content = $"{dayfood.Count} Food(s)";
                }

                var daydrink = List_d.FindAll(x => x.ExpiryTime.Value.Date == ((DateTime)currentday.Tag).Date);

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

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Button NextMonth Click Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_next_Click(object sender, MouseButtonEventArgs e)
        {
            _currentmonth = _currentmonth.AddMonths(1);

            InitLists();

            SetCalendar();
        }

        /// <summary>
        /// Button PreviousMonth Click Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_previous_Click(object sender, MouseButtonEventArgs e)
        {
            _currentmonth = _currentmonth.AddMonths(-1);

            InitLists();

            SetCalendar();
        }

        /// <summary>
        /// Loads Items to lists which expire in currentmonth
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<T> loadItems<T>() where T : Supplies,new()
        {
            var dbhelper = _container.Resolve<IDatabase>();
            List<T> list = dbhelper.Get<T>(_currentmonth);
            return list;
        }

        #endregion Private Methods
    }
}