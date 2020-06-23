using csharp_project.Data;
using csharp_project.DataAccess;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MyLog;
using Unity;

namespace csharp_project.Views
{
    /// <summary>
    /// Interaktionslogik für UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Window
    {
        #region Private Fields

        private readonly UnityContainer _container;

        #endregion Private Fields

        #region Public Constructors

        public UpdateDialog(UnityContainer container)
        {
            InitializeComponent();
            _container = container;
        }

        /// <summary>
        /// own constructor to get item to change
        /// </summary>
        /// <param name="id">Item to update</param>
        /// <param name="type">Food or Drinks</param>
        public UpdateDialog(int id, string type, UnityContainer container)
        {
            InitializeComponent();

            _container = container;

            d_update.Items.Clear();

            var dbhelper = _container.Resolve<IDatabase>();

            if (type == "Food")
            {
                List<Food> list = new List<Food>();

                var data = dbhelper.Get<Food>(id);

                if (data.Expires)
                    data.Lasting = (data.ExpiryTime - DateTime.Now).Value.Days;
                else data.Lasting = null;

                DataGridTextColumn textcol = new DataGridTextColumn();
                Binding b = new Binding("weigth");
                //Set the properties on the new column
                textcol.Binding = b;
                textcol.Header = "Weigth in g";
                textcol.Width = 80;

                d_update.Columns.Add(textcol);

                list.Add(data);

                d_update.ItemsSource = list;
            }
            else if (type == "Drinks")
            {
                List<Drinks> list = new List<Drinks>();

                var data = dbhelper.Get<Drinks>(id);

                if (data.Expires)
                    data.Lasting = (data.ExpiryTime - DateTime.Now).Value.Days;
                else
                    data.Lasting = null;

                DataGridTextColumn textcol = new DataGridTextColumn();
                Binding b = new Binding("volumen");
                //Set the properties on the new column
                textcol.Binding = b;
                textcol.Header = "Volumen";
                textcol.Width = 80;

                d_update.Columns.Add(textcol);

                list.Add(data);

                d_update.ItemsSource = list;
            }
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// Button Click Event Handler
        /// Stores changes to item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dbhelper = _container.Resolve<IDatabase>();

            if (d_update.SelectedItem.GetType().Name == "Food")
                dbhelper.Update(d_update.SelectedItem as Food);
            else if (d_update.SelectedItem.GetType().Name == "Drinks")
                dbhelper.Update(d_update.SelectedItem as Drinks);

            _container.Resolve<Log>().WriteLog($"Updated Item: {d_update.SelectedItem}");

            Close();
        }

        #endregion Private Methods
    }
}