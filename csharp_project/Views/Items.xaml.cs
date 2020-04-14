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
            var food_l= dbhelper.GetTable<Food>();
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
    }
}
