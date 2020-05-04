﻿using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using csharp_project.Speech;
using MyLog;

namespace csharp_project
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            this.DataContext = SpeechSynthesis.Instance;
        }

        private void Delete_DB_Click(object sender, RoutedEventArgs e)
        {
            Log.WriteLog($"Database deleted.");
            ShowLabelFaded(l_settings, "Database deletion successfully!");
            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.DeleteDatabase();
            dbhelper.CheckAndLoadDefaults();
        }

        private void Delete_Log_Click(object sender, RoutedEventArgs e)
        {
            ShowLabelFaded(l_settings, "Log deleted successfully!");
            Log.Delete();
            Log.CreateLogFile();
        }

        private void ShowLabelFaded(Label label, string s)
        {
            label.Content = s;
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

        private void Open_Log_Click(object sender, RoutedEventArgs e)
        {
            Log.OpenLog();
        }

        private void b_add_Click(object sender, RoutedEventArgs e)
        {
            SpeechSynthesis.Instance.UnloadCustomGrammar();
            SpeechSynthesis.Choices.Add(tb_command.Text);
            SpeechSynthesis.Instance.LoadCustomGrammar();
            SpeechSynthesis.Instance.StoreGrammar();
            Log.WriteLog($"Speech Command {tb_command.Text} added.");
        }

        private void b_delete_Click(object sender, RoutedEventArgs e)
        {
            if(SpeechSynthesis.Choices.Contains(tb_command.Text))
            {
                SpeechSynthesis.Choices.Remove(tb_command.Text);
                Log.WriteLog($"Speech Command {tb_command.Text} deleted.");
            }
            else
            {
                ShowLabelFaded(l_settings, "Command not found.");
            }

        }

        private void tb_command_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_command.Text != "Command")
                b_add.IsEnabled = true;
            else
                b_add.IsEnabled = false;
        }
    }
}
