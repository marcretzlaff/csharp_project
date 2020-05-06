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
using csharp_project.Speech;
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

            SpeechSynthesis speech = SpeechSynthesis.Instance;
            speech.LoadDefault();
        }

        /// <summary>
        /// ListView MousEnter Event Handler
        /// decides which Tooltip View is visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// ToggleButton Unchecked Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        /// <summary>
        /// ToggleButton Checked Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        /// <summary>
        /// Background LeftButtonDown Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        /// <summary>
        /// Close Button Click Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region set usercontrols
        /// <summary>
        /// Sets parameter control to visbile
        /// </summary>
        /// <param name="control"></param>
        private void SetActiveUserControl(UserControl control)
        {
            app_name.Visibility = Visibility.Collapsed;
            home.Visibility = Visibility.Collapsed;
            items.Visibility = Visibility.Collapsed;
            settings.Visibility = Visibility.Collapsed;
            calendar.Visibility = Visibility.Collapsed;

            control.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// ListViewItem MouseLeftButtonDown Event Handler
        /// View Home
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(home);
        }

        /// <summary>
        /// ListViewItem MouseLeftButtonDown Event Handler
        /// View Items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(items);
            items.LoadTables();
        }

        /// <summary>
        /// ListViewItem MouseLeftButtonDown Event Handler
        /// View calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(calendar);
            calendar.InitLists();
            calendar.SetCalendar();
        }

        /// <summary>
        /// ListViewItem MouseLeftButtonDown Event Handler
        /// View Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            SetActiveUserControl(settings);
        }

        #endregion set usercontrols

        /// <summary>
        /// Thumb DragDelta Event Handler
        /// HelperFuntction to move window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }
    }
}
