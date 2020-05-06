using csharp_project.Data;
using csharp_project.DataAccess;
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
using MyLog;

namespace csharp_project.Views
{
    /// <summary>
    /// Interaktionslogik für UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Window
    {
        public UpdateDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// own constructor to get item to change
        /// </summary>
        /// <param name="id">Item to update</param>
        /// <param name="type">Food or Drinks</param>
        public UpdateDialog(int id, string type)
        {
            InitializeComponent();

            d_update.Items.Clear();

            var dbhelper = DataManager.getInstance();
            if (type == "Food")
            {
                List<Food> list = new List<Food>();
                var data = dbhelper.Get<Food>(id);
                if (data.expires)
                    data.lasting = (data.expiryTime - DateTime.Now).Value.Days;
                else data.lasting = null;

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
                if (data.expires)
                    data.lasting = (data.expiryTime - DateTime.Now).Value.Days;
                else data.lasting = null;

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

        /// <summary>
        /// Button Click Event Handler
        /// Stores changes to item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dbhelper = DataManager.getInstance();

            if(d_update.SelectedItem.GetType().Name == "Food")
             dbhelper.Update(d_update.SelectedItem as Food);
            else if (d_update.SelectedItem.GetType().Name == "Drinks")
             dbhelper.Update(d_update.SelectedItem as Drinks);
            Log.WriteLog($"Updated Item: {d_update.SelectedItem.ToString()}");
            Close();
        }
    }
}
