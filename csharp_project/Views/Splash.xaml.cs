using csharp_project.DataAccess;
using csharp_project.Speech;
using MyLog;
using System;
using System.Windows;
using Unity;
using Unity.Injection;

namespace csharp_project.Views
{
    /// <summary>
    /// Interaktionslogik für SplashScreen.xaml
    /// </summary>
    public partial class Splash : Window
    {
        #region Public Constructors

        public Splash()
        {
            InitializeComponent();

            ContentRendered += onLoad;
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// Runs after Splash screen (Loading Screen) is shown
        /// Loads Unity Containers, database and speech recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            Hide();
        }

        #endregion Private Methods
    }
}