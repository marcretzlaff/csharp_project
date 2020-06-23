using csharp_project.Data;
using csharp_project.DataAccess;
using csharp_project.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für Items.xaml
    /// </summary>
    public partial class Items : UserControl
    {
        #region Private Fields

        private readonly UnityContainer _container;

        #endregion Private Fields

        #region Public Constructors

        public Items(UnityContainer container)
        {
            InitializeComponent();

            _container = container;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Run on startup and update to get current data.
        /// </summary>
        public void LoadTables()
        {
            var dbhelper = _container.Resolve<IDatabase>();
            var food_l = dbhelper.GetTable<Food>();

            foreach (var x in food_l)
            {
                if (x.Expires)
                    x.Lasting = (x.ExpiryTime - DateTime.Now).Value.Days + 1;
                else
                    x.Lasting = null;
            }

            d_food.ItemsSource = food_l;

            var drinks_l = dbhelper.GetTable<Drinks>();

            foreach (var x in drinks_l)
            {
                if (x.Expires)
                    x.Lasting = (x.ExpiryTime - DateTime.Now).Value.Days + 1;
                else
                    x.Lasting = null;
            }

            d_drinks.ItemsSource = drinks_l;
        }

        #endregion Public Methods

        #region Food

        /// <summary>
        /// MenuItem Table Food Action Copy Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemFood_Copy(object sender, RoutedEventArgs e)
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
        /// MenuItem Table Food Action Delete Handler
        /// Reloads table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemFood_Del(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext;

            _container.Resolve<IDatabase>().Delete<Food>((item as Food).Id);

            LoadTables();
        }

        /// <summary>
        /// MenuItem Table Food Action Info Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemFood_Info(object sender, RoutedEventArgs e)
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
        /// MenuItem Table Food Action Update Handler
        /// Opens update dialog and reloads table on dialog close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemFood_Update(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            UpdateDialog dia = new UpdateDialog((contextMenu.DataContext as Food).Id, "Food", _container);
            dia.ShowDialog();

            LoadTables(); //update List
        }

        #endregion Food

        #region Drinks

        /// <summary>
        /// MenuItem Table Drinks Action Copy Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDrinks_Copy(object sender, RoutedEventArgs e)
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
        /// MenuItem Table Drinks Action delete Handler
        /// Reloads table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDrinks_Del(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext;

            _container.Resolve<IDatabase>().Delete<Food>((item as Drinks).Id);
        }

        /// <summary>
        /// MenuItem Table Drinks Action Info Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDrinks_Info(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext as Drinks;

            if (item.Expires)
            {
                MessageBox.Show(item.GetInformation() + $"It expires in {(item.ExpiryTime.Value.Date - item.InsertTime.Date).TotalDays} days. Volume is {item.Volumen} mL.", "Information of Item");
            }
            else
            {
                MessageBox.Show(item.GetInformation(), "Information of Item");
            }
        }
        /// <summary>
        /// MenuItem Table Drinks Action Update Handler 
        /// reloads table on dialog close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDrinks_Update(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            UpdateDialog dia = new UpdateDialog((contextMenu.DataContext as Drinks).Id, "Drinks", _container);
            dia.ShowDialog();

            LoadTables(); //update List
        }
        #endregion Drinks
    }
}