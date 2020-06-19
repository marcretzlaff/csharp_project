using csharp_project.Views;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;

namespace csharp_project.Calendar
{
    /// <summary>
    /// Interaktionslogik für DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        private UnityContainer _container;
        public Calendar parent { get; private set; }
        public DayControl( UnityContainer container)
        {
            InitializeComponent();
            _container = container;
        }

        /// <summary>
        /// custom contructor to have simplified parent handling
        /// </summary>
        /// <param name="source"></param>
        public DayControl( Calendar source, UnityContainer container)
        {
            InitializeComponent();
            _container = container;
            parent = source;
        }

        /// <summary>
        /// MouseLeftButtonDown Event Handler Food
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void l_food_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var list = parent.list_f.FindAll(x => x.expiryTime.Value.Date == ((DateTime)Tag));
            if (list.Count == 0)
                return;
            CalendarItemsDialog dia = new CalendarItemsDialog("Food", this, _container);
            dia.ShowDialog();
        }
        /// <summary>
        /// MouseLeftButtonDown Event Handler Drinks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void l_drinks_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var list = parent.list_d.FindAll(x => x.expiryTime.Value.Date == ((DateTime)Tag));
            if (list.Count == 0)
                return;
            CalendarItemsDialog dia = new CalendarItemsDialog("Drinks", this, _container);
            dia.ShowDialog();
        }
    }
}
