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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für Items.xaml
    /// </summary>
    public partial class Items : UserControl
    {
        public Items()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dbhelper = DataAccess.DataManager.getInstance();
            var food_l = dbhelper.GetTable<Food>();
            foreach (var x in food_l)
            {
                x.lasting = (x.expiryTime - x.insertTime).Value.Days;
            }
            d_food.ItemsSource = food_l;

            var drinks_l = dbhelper.GetTable<Drinks>();
            foreach (var x in drinks_l)
            {
                x.lasting = (x.expiryTime - x.insertTime).Value.Days;
            }
            d_drinks.ItemsSource = drinks_l;
        }

        private void MenuItemFood_Info(object sender, RoutedEventArgs e)
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

        private void MenuItemFood_Del(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext;

            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.Delete<Food>((item as Food).Id);
        }

        private void MenuItemFood_Copy(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext.ToString();
            MessageBox.Show(item, "Copied to Clipboard!");
            Clipboard.SetText(item);
        }

        private void MenuItemDrinks_Info(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext as Drinks;
            if (item.expires)
            {
                MessageBox.Show(item.Getinformation() + $"It expires in {(item.expiryTime.Value.Date - item.insertTime.Date).TotalDays} days. Volume is {item.volumen} mL.", "Information of Item");
            }
            else
            {
                MessageBox.Show(item.Getinformation(), "Information of Item");
            }
        }

        private void MenuItemDrinks_Del(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext;

            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.Delete<Food>((item as Drinks).Id);
        }

        private void MenuItemDrinks_Copy(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext.ToString();
            MessageBox.Show(item, "Copied to Clipboard!");
            Clipboard.SetText(item);
        }
    }
}
