using csharp_project.Speech;
using MyLog;
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
            this.ContentRendered += onLoad;
        }

        private void onLoad(object sender, EventArgs e)
        {
            //DB setup
            Task db = Task.Run(() => DataAccess.DataManager.getInstance().CheckAndLoadDefaults());
            Task l  = Task.Run(() => Log.CreateLogFile());
            SpeechSynthesis.Instance.LoadDefault();
            var mainview = new MainWindow();
            mainview.Show();
            this.Hide();
        }
    }
}
