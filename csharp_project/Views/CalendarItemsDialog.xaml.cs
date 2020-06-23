using csharp_project.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace csharp_project.Views
{
    /// <summary>
    /// Interaktionslogik für CalendarItemsDialog.xaml
    /// </summary>
    public partial class CalendarItemsDialog : Window
    {
        #region Private Fields

        private readonly UnityContainer _container;
        private readonly string _listtyp;
        private readonly Calendar.DayControl _parent;

        #endregion Private Fields

        #region Public Constructors

        public CalendarItemsDialog(UnityContainer container)
        {
            InitializeComponent();

            _container = container;
        }

        /// <summary>
        /// own contstructor to get Item typ and day
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="parent"></param>
        public CalendarItemsDialog(string typ, Calendar.DayControl parent, UnityContainer container)
        {
            InitializeComponent();

            _listtyp = typ;
            _parent = parent;
            _container = container;

            FillList();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Fills lists with items on Day _parent
        /// </summary>
        public void FillList()
        {
            if(_listtyp == "Food")
                d_items.ItemsSource = _parent.Owner.List_f.FindAll(x => x.ExpiryTime.Value.Date == ((DateTime)_parent.Tag).Date);

            if(_listtyp == "Drinks")
                d_items.ItemsSource = _parent.Owner.List_d.FindAll(x => x.ExpiryTime.Value.Date == ((DateTime)_parent.Tag).Date);
        }

        #endregion Public Methods

        #region MenuItem
        /// <summary>
        /// Menu Item Copy Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Copy(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext.ToString();
            MessageBox.Show(item, "Copied to Clipboard!");
            Clipboard.SetText(item);
        }

        /// <summary>
        /// MenuItem Delete Event Handler
        /// reloads calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Del(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext;

            _container.Resolve<DataAccess.IDatabase>().Delete<Food>((item as Food).Id);

            _parent.Owner.InitLists();
            _parent.Owner.SetCalendar();

            FillList();
        }

        /// <summary>
        /// MenuItem Info Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Info(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext as Food;

            if (item.Expires)
            {
                MessageBox.Show(item.GetInformation() + $"It expires in {(item.ExpiryTime.Value.Date - item.InsertTime.Date).TotalDays} days. It weigths {item.Weigth} grams.", "Information of Item");
            }
            else
            {
                MessageBox.Show(item.GetInformation(), "Information of Item");
            }
        }
        /// <summary>
        /// MenuItem Update Event Handler
        /// reloads calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Update(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            UpdateDialog dia = new UpdateDialog(_container);
            if (_listtyp == "Food")
                dia = new UpdateDialog((contextMenu.DataContext as Food).Id, "Food", _container);

            if (_listtyp == "Drinks")
                dia = new UpdateDialog((contextMenu.DataContext as Drinks).Id, "Drinks", _container);

            dia.ShowDialog();

            //remake Calendar
            _parent.Owner.InitLists();
            _parent.Owner.SetCalendar();

            FillList();
        }

        #endregion MenuItem
    }
}