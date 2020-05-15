using csharp_project.Views;
using System.Windows.Controls;
using System.Windows.Input;

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

        /// <summary>
        /// custom contructor to have simplified parent handling
        /// </summary>
        /// <param name="source"></param>
        public DayControl( Calendar source)
        {
            InitializeComponent();
            parent = source;
        }

        /// <summary>
        /// MouseLeftButtonDown Event Handler Food
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void l_food_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalendarItemsDialog dia = new CalendarItemsDialog("Food", this);
            dia.ShowDialog();
        }
        /// <summary>
        /// MouseLeftButtonDown Event Handler Drinks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void l_drinks_count_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalendarItemsDialog dia = new CalendarItemsDialog("Drinks", this);
            dia.ShowDialog();
        }
    }
}
