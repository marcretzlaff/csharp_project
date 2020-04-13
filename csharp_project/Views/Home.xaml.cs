using csharp_project.Data;
using csharp_project.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaktionslogik für Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dbhelper = DataManager.getInstance();

            if ( dropdown_itemtyp.SelectedItem.ToString() == "Food")
            {
                Food data = null;
                if (expire_checkb.IsChecked.Value)
                {
                    //complicated item
                    data = new Food(item_tb.Text, date_added.SelectedDate.Value, date_until.SelectedDate.Value, Int32.Parse(tb_seize.Text));
                }
                else //simple item
                {
                    data = new Food(item_tb.Text);
                }

                if(dbhelper.Insert<Food>(data))
                {
                    //do
                }
                else
                {
                    //do
                }
            }
            else
            {
                Drinks data = null;
                if (expire_checkb.IsChecked.Value)
                {
                    data = new Drinks(item_tb.Text, date_added.SelectedDate.Value, date_until.SelectedDate.Value, Int32.Parse(tb_seize.Text));
                }
                else
                {
                    data = new Drinks(item_tb.Text);
                }

                if (dbhelper.Insert<Drinks>(data))
                {
                    //do
                }
                else
                {
                    //do
                }
            }
        }

        private void expire_checkb_Unchecked(object sender, RoutedEventArgs e)
        {
            date_until.Visibility = Visibility.Hidden;
        }

        private void expire_checkb_Checked(object sender, RoutedEventArgs e)
        {
            date_until.Visibility = Visibility.Visible;
        }

        private void tb_seize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tb_seize_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                Regex regex = new Regex("[^0-9]+");
                if (regex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void item_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (item_tb.Text != "Item Name")
            {
                add_item_bt.IsEnabled = true;
            }
        }
    }
}
