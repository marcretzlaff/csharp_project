using csharp_project.Data;
using csharp_project.DataAccess;
using csharp_project.Views;
using MyLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Unity;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        #region Private Fields

        private readonly UnityContainer _container;

        #endregion Private Fields

        #region Public Constructors

        public Home(UnityContainer container)
        {
            InitializeComponent();

            _container = container;
        }

        #endregion Public Constructors

        #region Search

        /// <summary>
        /// Button Search Click Event Handler
        /// Decides wether to search in databse with ID or Name/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_search_item_Click(object sender, RoutedEventArgs e)
        {
            var dbhelper = _container.Resolve<IDatabase>();

            if (dd_search_itemtyp.Text == "Food")
            {
                if (rb_id.IsChecked.Value)
                {
                    List<Food> list = new List<Food>();

                    if (int.TryParse(tb_search_id.Text, out int id))
                    {
                        try
                        {
                            Food data = dbhelper.Get<Food>(id);

                            if (data.Expires)
                                data.Lasting = (data.ExpiryTime - DateTime.Now).Value.Days;
                            else
                                data.Lasting = null;

                            list.Add(data);

                            d_search.ItemsSource = list;
                        }
                        catch (Exception ex)
                        {
                            _container.Resolve<Log>().WriteException(ex, "Home: Searched key invalid.");
                            showLabelFaded(l_adding, "ERROR: ID not in database.");
                        }
                    }
                    else
                        showLabelFaded(l_adding, "ERROR: ID could not be converted to INT.");
                }
                else if (rb_name.IsChecked.Value)
                {
                    var food_l = dbhelper.Get<Food>(tb_search_name.Text);

                    foreach (var x in food_l)
                    {
                        if (x.Expires)
                            x.Lasting = (x.ExpiryTime - DateTime.Now).Value.Days;
                        else
                            x.Lasting = null;
                    }

                    d_search.ItemsSource = food_l;
                }
            }
            else if (dd_search_itemtyp.Text == "Drinks")
            {
                if (rb_id.IsChecked.Value)
                {
                    List<Drinks> list = new List<Drinks>();

                    if (int.TryParse(tb_search_id.Text, out int id))
                    {
                        try
                        {
                            Drinks data = dbhelper.Get<Drinks>(id);

                            if (data.Expires)
                                data.Lasting = (data.ExpiryTime - DateTime.Now).Value.Days;
                            else
                                data.Lasting = null;

                            list.Add(data);

                            d_search.ItemsSource = list;
                        }
                        catch (Exception ex)
                        {
                            _container.Resolve<Log>().WriteException(ex, "Home: Searched key invalid.");
                            showLabelFaded(l_adding, "ERROR: ID not in database.");
                        }
                    }
                    else
                        showLabelFaded(l_adding, "ID could not be converted to INT.");
                }
                else if (rb_name.IsChecked.Value)
                {
                    var drinks_l = dbhelper.Get<Drinks>(tb_search_name.Text);

                    foreach (var x in drinks_l)
                    {
                        if (x.Expires)
                            x.Lasting = (x.ExpiryTime - DateTime.Now).Value.Days;
                        else
                            x.Lasting = null;
                    }

                    d_search.ItemsSource = drinks_l;
                }
            }
        }

        /// <summary>
        /// Button Update Click Event Handler
        /// Opens Update Dialog on selected Item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_search_update_Click(object sender, RoutedEventArgs e)
        {
            var dbhelper = _container.Resolve<IDatabase>();

            if (dd_search_itemtyp.Text == "Food")
            {
                List<Food> list = new List<Food>();

                try
                {
                    int id = (d_search.SelectedItem as Food).Id;

                    UpdateDialog dia = new UpdateDialog(id, "Food", _container);
                    dia.ShowDialog();

                    var data = dbhelper.Get<Food>(id);
                    list.Add(data);

                    d_search.ItemsSource = list;
                }
                catch (Exception ex)
                {
                    _container.Resolve<Log>().WriteException(ex, "Unsuccessful update in Home Screen.");
                }
            }
            else if (dd_search_itemtyp.Text == "Drinks")
            {
                List<Drinks> list = new List<Drinks>();

                try
                {
                    int id = (d_search.SelectedItem as Drinks).Id;

                    UpdateDialog dia = new UpdateDialog(id, "Drinks", _container);
                    dia.ShowDialog();

                    var data = dbhelper.Get<Drinks>(id);
                    list.Add(data);

                    d_search.ItemsSource = list;
                }
                catch (Exception ex)
                {
                    _container.Resolve<Log>().WriteException(ex, "Unsuccessful update in Home Screen.");
                }
            }

            showLabelFaded(l_adding, "Item updated!");
        }

        /// <summary>
        /// DataGrid CellChanged Event Handler
        /// Activates Update Button to store changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void d_search_CurrentCellChanged(object sender, EventArgs e)
        {
            b_search_update.IsEnabled = true;
        }

        /// <summary>
        /// Textbox SearchID Preview Event Handler
        /// Only accpets digits 0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_search_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion Search

        #region Adding
        /// <summary>
        /// Expire Checkbox Checked Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_adding_expire_Checked(object sender, RoutedEventArgs e)
        {
            dp_adding_date_until.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Expire Checkbox Uncheck Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_adding_expire_Unchecked(object sender, RoutedEventArgs e)
        {
            dp_adding_date_until.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Adding Button: Adding item to database. Logic decides if it's a simple or complicated item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_adding_item_Click(object sender, RoutedEventArgs e)
        {
            bool success;
            int mul = 1;

            var dbhelper = _container.Resolve<IDatabase>();
            success = int.TryParse(tb_adding_size.Text, out int size);

            if (success)
            {
                if (size < 0)
                    size = 0;

                success = int.TryParse(tb_adding_mul.Text, out mul);
            }
            else
            {
                size = 1;
                success = int.TryParse(tb_adding_mul.Text, out mul);
            }

            if (success)
            {
                if (mul <= 0)
                    mul = 1;

                if (dd_adding_itemtyp.Text == "Food")
                {
                    Food data;

                    if (b_adding_expire.IsChecked.Value)
                    {
                        //complicated item
                        data = new Food(tb_adding_name.Text, dp_adding_date_added.SelectedDate.Value, dp_adding_date_until.SelectedDate.Value, size);
                    }
                    else //simple item
                    {
                        data = new Food(tb_adding_name.Text, size);
                    }

                    while (mul-- != 0)
                    {
                        success = dbhelper.Insert<Food>(data);

                        if (!success)
                            break;
                    }
                }
                else
                {
                    Drinks data;

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
                }
            }

            if (success)
            {
                showLabelFaded(l_adding, "Item added successfully to Database!");
            }
            else
            {
                showLabelFaded(l_adding, "Adding to Database failed!");
            }
        }

        /// <summary>
        /// Textbox number of items Pasting Event Handler
        /// Only accpets digits 0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_adding_mul_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = e.DataObject.GetData(typeof(string)).ToString();

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

        /// <summary>
        /// Textbox number of items Preview Event Handler
        /// Only accpets digits 0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_adding_mul_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Textbox Name of Item TextChanged Event Handler
        /// Enables Add Button if Text varies from default.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Textbox Size Pasting Handler
        /// Only paste text containing digits 0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_adding_size_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = e.DataObject.GetData(typeof(string)).ToString();

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

        /// <summary>
        /// Textbox Size Preview Event Handler
        /// Only accpets digits 0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_adding_size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion Adding

        #region Extras
        /// <summary>
        /// Helper funtcion to display temporyry label which fades out after 3s
        /// </summary>
        /// <param name="label"> Label to activate on</param>
        /// <param name="s"> Content of label</param>
        private void showLabelFaded(Label label, string s)
        {
            label.Content = s;
            label.Visibility = Visibility.Visible;

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
            storyboard.Completed += delegate { label.Visibility = Visibility.Hidden; };
            storyboard.Begin();
        }

        #endregion Extras
    }
}