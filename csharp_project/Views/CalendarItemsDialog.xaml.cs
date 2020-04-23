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
        public CalendarItemsDialog()
        {
            InitializeComponent();
        }

        public CalendarItemsDialog(List<Food> list, string typ)
        {
            InitializeComponent();
            d_items.ItemsSource = list;
            _listtyp = typ;
        }

        public CalendarItemsDialog(List<Drinks> list, string typ)
        {
            InitializeComponent();
            d_items.ItemsSource = list;
            _listtyp = typ;
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
            
            //remake Calendar!!!!
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

            //remake Calendar!!!!
        }
    }
}
