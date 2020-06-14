using csharp_project.DataAccess;
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
using Unity;
using Unity.Injection;

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
            var container = new UnityContainer();
            container.RegisterSingleton<Log>();
            container.RegisterType<IDatabase, DataManager>(TypeLifetime.Singleton, new InjectionProperty("Container", container));
            container.RegisterSingleton<SpeechSynthesis>(new InjectionProperty("Container", container));

            container.Resolve<IDatabase>().CheckAndLoadDefaults();
            container.Resolve<Log>().CreateLogFile();
            container.Resolve<SpeechSynthesis>().LoadDefault();

            var mainview = new MainWindow(container);
            mainview.Show();
            this.Hide();
        }
    }
}
