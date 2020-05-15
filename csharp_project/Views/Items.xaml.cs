﻿using csharp_project.Data;
using csharp_project.Views;
using System;
using System.Windows;
using System.Windows.Controls;

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
        /// <summary>
        /// Run on startup and update to get current data.
        /// </summary>
        public void LoadTables()
        {
            var dbhelper = DataAccess.DataManager.getInstance();
            var food_l = dbhelper.GetTable<Food>();
            foreach (var x in food_l)
            {
                if (x.expires)
                    x.lasting = (x.expiryTime - DateTime.Now).Value.Days + 1;
                else x.lasting = null;
            }
            d_food.ItemsSource = food_l;

            var drinks_l = dbhelper.GetTable<Drinks>();
            foreach (var x in drinks_l)
            {
                if (x.expires)
                    x.lasting = (x.expiryTime - DateTime.Now).Value.Days + 1;
                else x.lasting = null;
            }
            d_drinks.ItemsSource = drinks_l;
        }

        #region Food

        /// <summary>
        /// MenuItem Table Food Action Info Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// MenuItem Table Food Action Delete Handler
        /// Reloads table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemFood_Del(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            var item = contextMenu.DataContext;

            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.Delete<Food>((item as Food).Id);

            LoadTables();
        }

        /// <summary>
        /// MenuItem Table Food Action Copy Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// MenuItem Table Food Action Update Handler
        /// Opens update dialog and reloads table on dialog close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemFood_Update(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            UpdateDialog dia = new UpdateDialog((contextMenu.DataContext as Food).Id, "Food");
            dia.ShowDialog();
            LoadTables(); //update List
        }

        #endregion Food

        #region Drinks

        /// <summary>
        /// MenuItem Table Drinks Action Info Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// MenuItem Table Drinks Action delete Handler
        /// Reloads table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// MenuItem Table Drinks Action Copy Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// MenuItem Table Drinks Action Update Handler 
        /// reloads table on dialog close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDrinks_Update(object sender, RoutedEventArgs e)
        {
            // Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;

            UpdateDialog dia = new UpdateDialog((contextMenu.DataContext as Drinks).Id, "Drinks");
            dia.ShowDialog();
            LoadTables(); //update List
        }
        #endregion Drinks
    }
}
