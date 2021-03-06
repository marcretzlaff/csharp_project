﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using csharp_project.DataAccess;
using csharp_project.Speech;
using MyLog;
using Unity;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        #region Private Fields

        private readonly UnityContainer _container;

        #endregion Private Fields

        #region Public Constructors

        public Settings(UnityContainer container)
        {
            InitializeComponent();

            _container = container;

            DataContext = _container.Resolve<SpeechSynthesis>();

            if (Properties.Settings.Default.SpeechActivated)
                cb_speech_activ.IsChecked = true;
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// Adds Speech recognition choice to custom grammar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_add_Click(object sender, RoutedEventArgs e)
        {
            var speech = _container.Resolve<SpeechSynthesis>();

            if (!speech.Choices.Contains(tb_command.Text))
            {
                speech.UnloadCustomGrammar();
                speech.Choices.Add(tb_command.Text);

                speech.LoadCustomGrammar();
                speech.StoreGrammar();

                _container.Resolve<Log>().WriteLog($"Speech Command {tb_command.Text} added.");

                tb_command.Text = "";
            }
            else
            {
                showLabelFaded(l_settings, "Command already in Grammar.");
            }
        }

        /// <summary>
        /// deletes speech recognition chouice from custom grammar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_delete_Click(object sender, RoutedEventArgs e)
        {
            var sel = LV_commands.SelectedItem?.ToString();
            var speech = _container.Resolve<SpeechSynthesis>();

            if (speech.Choices.Contains(sel) && sel != null)
            {
                speech.Choices.Remove(sel);

                _container.Resolve<Log>().WriteLog($"Speech Command \"{sel}\" deleted.");

                speech.UnloadCustomGrammar();
                speech.LoadCustomGrammar();
                speech.StoreGrammar();
            }
            else
            {
                showLabelFaded(l_settings, "Select a item and try again.");
            }
        }

        /// <summary>
        /// Deletes current SQLite database and loads default new one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_DB_Click(object sender, RoutedEventArgs e)
        {
            _container.Resolve<Log>().WriteLog($"Database deleted.");

            showLabelFaded(l_settings, "Database deletion successfully!");

            var dbhelper = _container.Resolve<IDatabase>();
            dbhelper.DeleteDatabase();
            dbhelper.CheckAndLoadDefaults();
        }

        /// <summary>
        /// deletes log file and creates new one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Log_Click(object sender, RoutedEventArgs e)
        {
            showLabelFaded(l_settings, "Log deleted successfully!");

            _container.Resolve<Log>().Delete();
            _container.Resolve<Log>().CreateLogFile();
        }

        /// <summary>
        /// Opens Log file for reading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_Log_Click(object sender, RoutedEventArgs e)
        {
            _container.Resolve<Log>().OpenLog();
        }

        /// <summary>
        /// Helper function shows fading text on button clicks
        /// </summary>
        /// <param name="label"></param>
        /// <param name="s"></param>
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

        /// <summary>
        /// filter for choice command text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_command_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tb_command.Text != "Command") && (tb_command.Text != ""))
                b_add.IsEnabled = true;
            else
                b_add.IsEnabled = false;
        }

        #endregion Private Methods

        #region CheckBoxHandler
        /// <summary>
        /// activates speech recogniton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_speech_activ_Checked(object sender, RoutedEventArgs e)
        {
            SpeechSynthesis speech = _container.Resolve<SpeechSynthesis>();
            speech.DeactivateSpeech();
            speech.LoadDefault();

            Properties.Settings.Default.SpeechActivated = true;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// deactivates Speec recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_speech_activ_Unchecked(object sender, RoutedEventArgs e)
        {
            _container.Resolve<SpeechSynthesis>().DeactivateSpeech();

            Properties.Settings.Default.SpeechActivated = false;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Loads regular dictionary to speech recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_speech_dict_Checked(object sender, RoutedEventArgs e)
        {
            _container.Resolve<SpeechSynthesis>().LoadRegularDict();
        }

        /// <summary>
        /// unloads regular dictionary to speech recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_speech_dict_Unchecked(object sender, RoutedEventArgs e)
        {
            SpeechSynthesis speech = _container.Resolve<SpeechSynthesis>();
            speech.UnloadRegularDict();
        }

        #endregion CheckBoxHandler

    }
}