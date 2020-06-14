using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using csharp_project.Speech;
using csharp_project.Views;
using csharp_project.Calendar;
using MyLog;
using Unity;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UnityContainer _container;

        Home home;
        Items items;
        Settings settings;
        Calendar.Calendar calendar;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(UnityContainer container)
        {
            InitializeComponent();
            _container = container;

            home = new Home(container);
            items = new Items(container);
            settings = new Settings(container);
            calendar = new Calendar.Calendar(container);

            onLoad();
        }

        /// <summary>
        /// Sets Views to corresponding grid not in xaml due the non default constructor
        /// </summary>
        private void onLoad()
        {
            home.Visibility = Visibility.Collapsed;
            Grid.SetColumn(home, 1);
            BG.Children.Add(home);

            items.Visibility = Visibility.Collapsed;
            Grid.SetColumn(items, 1);
            BG.Children.Add(items);

            settings.Visibility = Visibility.Collapsed;
            Grid.SetColumn(settings, 1);
            BG.Children.Add(settings);

            calendar.Visibility = Visibility.Collapsed;
            Grid.SetColumn(calendar, 1);
            BG.Children.Add(calendar);
        }

        /// <summary>
        /// ListView MousEnter Event Handler
        /// decides which Tooltip View is visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //Tooltip visability while navbar open
            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_items.Visibility = Visibility.Collapsed;
                tt_settings.Visibility = Visibility.Collapsed;
                tt_calendar.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_items.Visibility = Visibility.Visible;
                tt_settings.Visibility = Visibility.Visible;
                tt_calendar.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// ToggleButton Unchecked Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        /// <summary>
        /// ToggleButton Checked Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        /// <summary>
        /// Background LeftButtonDown Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bg_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        /// <summary>
        /// Close Button Click Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            _container.Resolve<SpeechSynthesis>();
            Close();
        }

        #region set usercontrols
        /// <summary>
        /// Sets parameter control to visbile
        /// </summary>
        /// <param name="control"></param>
        private void setActiveUserControl(UserControl control)
        {
            tb_app_name.Visibility = Visibility.Collapsed;
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
        private void listViewItem_PreviewMouseLeftButtonDown_home(object sender, MouseButtonEventArgs e)
        {
            setActiveUserControl(home);
        }

        /// <summary>
        /// ListViewItem MouseLeftButtonDown Event Handler
        /// View Items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewItem_PreviewMouseLeftButtonDown_items(object sender, MouseButtonEventArgs e)
        {
            setActiveUserControl(items);
            items.LoadTables();
        }

        /// <summary>
        /// ListViewItem MouseLeftButtonDown Event Handler
        /// View calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewItem_PreviewMouseLeftButtonDown_calendar(object sender, MouseButtonEventArgs e)
        {
            setActiveUserControl(calendar);
            calendar.InitLists();
            calendar.SetCalendar();
        }

        /// <summary>
        /// ListViewItem MouseLeftButtonDown Event Handler
        /// View Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewItem_PreviewMouseLeftButtonDown_settings(object sender, MouseButtonEventArgs e)
        {
            setActiveUserControl(settings);
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
