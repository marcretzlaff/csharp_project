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
        private UnityContainer _container;
        private string _listtyp;
        private Calendar.DayControl _parent;
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

        /// <summary>
        /// Fills lists with items on Day _parent
        /// </summary>
        public void FillList()
        {
            if(_listtyp == "Food")
                d_items.ItemsSource = _parent.parent.list_f.FindAll(x => x.expiryTime.Value.Date == ((DateTime)_parent.Tag).Date);
            if(_listtyp == "Drinks")
                d_items.ItemsSource = _parent.parent.list_d.FindAll(x => x.expiryTime.Value.Date == ((DateTime)_parent.Tag).Date);
        }

        #region MenuItem
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
            if (item.expires)
            {
                MessageBox.Show(item.GetInformation() + $"It expires in {(item.expiryTime.Value.Date - item.insertTime.Date).TotalDays} days. It weigths {item.weigth} grams.", "Information of Item");
            }
            else
            {
                MessageBox.Show(item.GetInformation(), "Information of Item");
            }
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

            _parent.parent.InitLists();
            _parent.parent.SetCalendar();
            FillList();
        }

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
            _parent.parent.InitLists();
            _parent.parent.SetCalendar();
            FillList();
        }

        #endregion MenuItem
    }
}
