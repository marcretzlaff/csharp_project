using csharp_project.Data;
using csharp_project.DataAccess;
using csharp_project.Views;
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
using System.Windows.Media.Animation;
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
        #region Search
        private void tb_search_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void d_search_CurrentCellChanged(object sender, EventArgs e)
        {
            b_search_update.IsEnabled = true;
        }

        private void b_search_update_Click(object sender, RoutedEventArgs e)
        {
            if (dd_adding_itemtyp.Text == "Food")
            {
                UpdateDialog dia = new UpdateDialog((d_search.SelectedItem as Food).Id, "Food");
                dia.Show();
            }
            else if (dd_adding_itemtyp.Text == "Drinks")
            {
                UpdateDialog dia = new UpdateDialog((d_search.SelectedItem as Food).Id, "Food");
                dia.Show();
            }

        }

        private void b_search_item_Click(object sender, RoutedEventArgs e)
        {
            var dbhelper = DataManager.getInstance();


            if (dd_adding_itemtyp.Text == "Food")
            {
                if (rb_id.IsChecked.Value)
                {
                    List<Food> list = new List<Food>();
                    Int32.TryParse(tb_search_id.Text, out int id);
                    Food data = dbhelper.Get<Food>(id);
                    if (data.expires)
                        data.lasting = (data.expiryTime - data.insertTime).Value.Days;
                    else data.lasting = null;
                    list.Add(data);
                    d_search.ItemsSource = list;
                }
                else if (rb_name.IsChecked.Value)
                {
                    var food_l = dbhelper.Get<Food>(tb_search_name.Text);
                    foreach (var x in food_l)
                    {
                        if (x.expires)
                            x.lasting = (x.expiryTime - x.insertTime).Value.Days;
                        else x.lasting = null;
                    }
                    d_search.ItemsSource = food_l;
                }
            }
            else if (dd_adding_itemtyp.Text == "Drinks")
            {
                if (rb_id.IsChecked.Value)
                {
                    List<Drinks> list = new List<Drinks>();
                    Int32.TryParse(tb_search_id.Text, out int id);
                    Drinks data = dbhelper.Get<Drinks>(id);
                    if (data.expires)
                        data.lasting = (data.expiryTime - data.insertTime).Value.Days;
                    else data.lasting = null;
                    list.Add(data);
                    d_search.ItemsSource = list;
                }
                else if (rb_name.IsChecked.Value)
                {
                    var drinks_l = dbhelper.Get<Drinks>(tb_search_name.Text);
                    foreach (var x in drinks_l)
                    {
                        if (x.expires)
                            x.lasting = (x.expiryTime - x.insertTime).Value.Days;
                        else x.lasting = null;
                    }
                    d_search.ItemsSource = drinks_l;
                }
            }
        }

        #endregion Search

        #region Adding
        private void b_adding_item_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;

            var dbhelper = DataManager.getInstance();
            Int32.TryParse(tb_adding_size.Text, out int size);
            Int32.TryParse(tb_adding_mul.Text, out int mul);
            if ( dd_adding_itemtyp.Text == "Food")
            {
                Food data = null;
                if (b_adding_expire.IsChecked.Value)
                {
                    //complicated item
                    data = new Food(tb_adding_name.Text, dp_adding_date_added.SelectedDate.Value, dp_adding_date_until.SelectedDate.Value, size);
                }
                else //simple item
                {
                    data = new Food(tb_adding_name.Text,size);
                }

                while (mul-- != 0)
                {
                    success = dbhelper.Insert<Food>(data);
                    if (!success) break;
                }
                if (success)
                {
                    ShowLabelFaded(l_success);
                }
                else
                {
                    ShowLabelFaded(l_failed);
                }
            }
            else
            {
                Drinks data = null;
                if (b_adding_expire.IsChecked.Value)
                {
                    data = new Drinks(tb_adding_name.Text, dp_adding_date_added.SelectedDate.Value, dp_adding_date_until.SelectedDate.Value, size);
                }
                else
                {
                    data = new Drinks(tb_adding_name.Text, size);
                }

                while (mul-- != 0)
                {
                    success = dbhelper.Insert<Drinks>(data);
                    if (!success) break;
                }
                if (success)
                {
                    ShowLabelFaded(l_success);
                }
                else
                {
                    ShowLabelFaded(l_failed);
                }
            }
        }

        private void b_adding_expire_Unchecked(object sender, RoutedEventArgs e)
        {
            dp_adding_date_until.Visibility = Visibility.Hidden;
        }

        private void b_adding_expire_Checked(object sender, RoutedEventArgs e)
        {
            dp_adding_date_until.Visibility = Visibility.Visible;
        }

        private void tb_adding_size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tb_adding_size_Pasting(object sender, DataObjectPastingEventArgs e)
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

        private void tb_adding_mul_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tb_adding_mul_Pasting(object sender, DataObjectPastingEventArgs e)
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
        private void tb_adding_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_adding_name.Text != "Item Name")
            {
                b_adding_item.IsEnabled = true;
            }
            else
            {
                b_adding_item.IsEnabled = false;
            }
        }
        #endregion Adding

        #region Extras
        private void ShowLabelFaded(Label label)
        {
            label.Visibility = System.Windows.Visibility.Visible;

            var a = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                FillBehavior = FillBehavior.Stop,
                BeginTime = TimeSpan.FromSeconds(2),
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            var storyboard = new Storyboard();

            storyboard.Children.Add(a);
            Storyboard.SetTarget(a, label);
            Storyboard.SetTargetProperty(a, new PropertyPath(OpacityProperty));
            storyboard.Completed += delegate { label.Visibility = System.Windows.Visibility.Hidden; };
            storyboard.Begin();
        }

        #endregion Extras
    }
}
