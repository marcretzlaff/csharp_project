using csharp_project.Calendar;
using csharp_project.Data;
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
using System.Windows.Shapes;

namespace csharp_project.Views
{
    /// <summary>
    /// Interaktionslogik für CalendarItemsDialog.xaml
    /// </summary>
    public partial class CalendarItemsDialog : Window
    {
        private string _listtyp;
        private Calendar.DayControl _parent;
        public CalendarItemsDialog()
        {
            InitializeComponent();
        }

        public CalendarItemsDialog(string typ, Calendar.DayControl parent)
        {
            InitializeComponent();
            _listtyp = typ;
            _parent = parent;
            FillList();
        }
        public void FillList()
        {
            if(_listtyp == "Food")
                d_items.ItemsSource = _parent.parent.list_f.FindAll(x => x.expiryTime.Value.Date == ((DateTime)_parent.Tag).Date);
            if(_listtyp == "Drinks")
                d_items.ItemsSource = _parent.parent.list_d.FindAll(x => x.expiryTime.Value.Date == ((DateTime)_parent.Tag).Date);
        }

        private void MenuItem_Info(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext as Food;
            if (item.expires)
            {
                MessageBox.Show(item.Getinformation() + $"It expires in {(item.expiryTime.Value.Date - item.insertTime.Date).TotalDays} days. It weigths {item.weigth} grams.", "Information of Item");
            }
            else
            {
                MessageBox.Show(item.Getinformation(), "Information of Item");
            }
        }

        private void MenuItem_Del(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext;

            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.Delete<Food>((item as Food).Id);

            _parent.parent.InitLists();
            _parent.parent.SetCalendar();
        }

        private void MenuItem_Copy(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext.ToString();
            MessageBox.Show(item, "Copied to Clipboard!");
            Clipboard.SetText(item);
        }

        private void MenuItem_Update(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            UpdateDialog dia = new UpdateDialog();
            if (_listtyp == "Food")
                dia = new UpdateDialog((contextMenu.DataContext as Food).Id, "Food");
            if(_listtyp == "Drinks")
                dia = new UpdateDialog((contextMenu.DataContext as Drinks).Id, "Drinks");
            dia.ShowDialog();

            //remake Calendar
            _parent.parent.InitLists();
            _parent.parent.SetCalendar();
            FillList();
        }
    }
}
