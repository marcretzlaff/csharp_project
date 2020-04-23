using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyLog;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //DB setup
            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.CheckAndLoadDefaults();
            Log.CreateLogFile();
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //Tooltip visability
            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_items.Visibility = Visibility.Collapsed;
                tt_settings.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_items.Visibility = Visibility.Visible;
                tt_settings.Visibility = Visibility.Visible;
            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetActiveUserControl(UserControl control)
        {
            app_name.Visibility = Visibility.Collapsed;
            home.Visibility = Visibility.Collapsed;
            items.Visibility = Visibility.Collapsed;
            settings.Visibility = Visibility.Collapsed;
            calendar.Visibility = Visibility.Collapsed;

            control.Visibility = Visibility.Visible;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(home);
        }

        private void ListViewItem_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(items);
            items.LoadTables();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(calendar);
            calendar.InitLists();
            calendar.SetCalendar();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(settings);
        }
        private void headerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }
    }
}
