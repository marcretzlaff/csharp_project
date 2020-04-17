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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;

            var dbhelper = DataManager.getInstance();
            Int32.TryParse(tb_size.Text, out int size);
            Int32.TryParse(tb_mul.Text, out int mul);
            if ( dropdown_itemtyp.Text == "Food")
            {
                Food data = null;
                if (expire_checkb.IsChecked.Value)
                {
                    //complicated item
                    data = new Food(item_tb.Text, date_added.SelectedDate.Value, date_until.SelectedDate.Value, size);
                }
                else //simple item
                {
                    data = new Food(item_tb.Text,size);
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
                if (expire_checkb.IsChecked.Value)
                {
                    data = new Drinks(item_tb.Text, date_added.SelectedDate.Value, date_until.SelectedDate.Value, size);
                }
                else
                {
                    data = new Drinks(item_tb.Text, size);
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

        private void expire_checkb_Unchecked(object sender, RoutedEventArgs e)
        {
            date_until.Visibility = Visibility.Hidden;
        }

        private void expire_checkb_Checked(object sender, RoutedEventArgs e)
        {
            date_until.Visibility = Visibility.Visible;
        }

        private void tb_size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tb_size_Pasting(object sender, DataObjectPastingEventArgs e)
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

        private void tb_mul_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tb_mul_Pasting(object sender, DataObjectPastingEventArgs e)
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
            else
            {
                add_item_bt.IsEnabled = false;
            }
        }


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
    }
}
