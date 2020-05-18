using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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
            if (Properties.Settings.Default.SpeechActivated) cb_speech_activ.IsChecked = true;
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
            if (!SpeechSynthesis.Choices.Contains(tb_command.Text))
            {
                SpeechSynthesis.Instance.UnloadCustomGrammar();
                SpeechSynthesis.Choices.Add(tb_command.Text);
                SpeechSynthesis.Instance.LoadCustomGrammar();
                SpeechSynthesis.Instance.StoreGrammar();
                Log.WriteLog($"Speech Command {tb_command.Text} added.");
            }
            else
            {
                ShowLabelFaded(l_settings, "Command already in Grammar.");
            }
        }

        private void b_delete_Click(object sender, RoutedEventArgs e)
        {
            var sel = LV_commands.SelectedItem?.ToString();
            if (SpeechSynthesis.Choices.Contains(sel) && sel != null)
            {
                SpeechSynthesis.Choices.Remove(sel);
                Log.WriteLog($"Speech Command {sel} deleted.");
                SpeechSynthesis.Instance.UnloadCustomGrammar();
                SpeechSynthesis.Instance.LoadCustomGrammar();
                SpeechSynthesis.Instance.StoreGrammar();
            }
            else
            {
                ShowLabelFaded(l_settings, "Select a item and try again.");
            }

        }

        private void tb_command_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_command.Text != "Command")
                b_add.IsEnabled = true;
            else
                b_add.IsEnabled = false;
        }

        #region CheckBoxHandler
        private void cb_speech_dict_Checked(object sender, RoutedEventArgs e)
        {
            SpeechSynthesis speech = SpeechSynthesis.Instance;
            speech.LoadRegularDict();
        }

        private void cb_speech_dict_Unchecked(object sender, RoutedEventArgs e)
        {
            SpeechSynthesis speech = SpeechSynthesis.Instance;
            speech.UnloadRegularDict();
        }

        private void cb_speech_activ_Checked(object sender, RoutedEventArgs e)
        {
            SpeechSynthesis speech = SpeechSynthesis.Instance;
            speech.LoadDefault();

            Properties.Settings.Default.SpeechActivated = true;
            Properties.Settings.Default.Save();
        }

        private void cb_speech_activ_Unchecked(object sender, RoutedEventArgs e)
        {
            SpeechSynthesis speech = SpeechSynthesis.Instance;
            speech.DeactivateSpeech();

            Properties.Settings.Default.SpeechActivated = false;
            Properties.Settings.Default.Save();
        }
        #endregion CheckBoxHandler

    }
}
