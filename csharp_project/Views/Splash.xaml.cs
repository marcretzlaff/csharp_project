﻿using csharp_project.Speech;
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
using System.Windows.Shapes;

namespace csharp_project.Views
{
    /// <summary>
    /// Interaktionslogik für SplashScreen.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            //DB setup
            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.CheckAndLoadDefaults();

            SpeechSynthesis speech = SpeechSynthesis.Instance;
            speech.LoadDefault();

            var mainview = new MainWindow();
            mainview.Show();
            this.Hide();
        }
    }
}
