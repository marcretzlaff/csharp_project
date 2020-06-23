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
        #region Private Fields

        private readonly UnityContainer _container;

        #endregion Private Fields

        #region Public Constructors

        public DayControl(UnityContainer container)
        {
            InitializeComponent();

            _container = container;
        }

        /// <summary>
        /// custom contructor to have simplified parent handling
        /// </summary>
        /// <param name="source"></param>
        public DayControl(Calendar source, UnityContainer container)
        {
            InitializeComponent();

            _container = container;
            Owner = source;
        }

        #endregion Public Constructors

        #region Public Properties

        public Calendar Owner { get; private set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// MouseLeftButtonDown Event Handler Drinks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void l_drinks_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var list = Owner.List_d.FindAll(x => x.ExpiryTime.Value.Date == ((DateTime)Tag));

            if (list.Count == 0)
                return;

            CalendarItemsDialog dia = new CalendarItemsDialog("Drinks", this, _container);
            dia.ShowDialog();
        }

        /// <summary>
        /// MouseLeftButtonDown Event Handler Food
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void l_food_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var list = Owner.List_f.FindAll(x => x.ExpiryTime.Value.Date == ((DateTime)Tag));

            if (list.Count == 0)
                return;

            CalendarItemsDialog dia = new CalendarItemsDialog("Food", this, _container);
            dia.ShowDialog();
        }

        #endregion Private Methods
    }
}