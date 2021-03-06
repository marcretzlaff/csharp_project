﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using csharp_project.Speech;
using Unity;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields

        private readonly UnityContainer _container;

        #endregion Private Fields

        #region Visible Views

        private readonly Calendar.Calendar calendar;
        private readonly Home home;
        private readonly Items items;
        private readonly Settings settings;

        #endregion Visible Views

        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(UnityContainer container) : this()
        {
            _container = container;

            home = new Home(container);
            items = new Items(container);
            settings = new Settings(container);
            calendar = new Calendar.Calendar(container);

            onLoad();
        }

        #endregion Public Constructors

        #region Private Methods

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
            _container.Resolve<SpeechSynthesis>().DeactivateSpeech();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Thumb DragDelta Event Handler
        /// HelperFuntction to move window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Left += e.HorizontalChange;
            Top += e.VerticalChange;
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
        /// ToggleButton Checked Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
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

        #endregion Private Methods

        #region set usercontrols
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
        /// View Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewItem_PreviewMouseLeftButtonDown_settings(object sender, MouseButtonEventArgs e)
        {
            setActiveUserControl(settings);
        }

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

        #endregion set usercontrols
    }
}
