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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        }

        private void Delete_DB_Click(object sender, RoutedEventArgs e)
        {
            Log.WriteLog($"Database deleted.");
            var dbhelper = DataAccess.DataManager.getInstance();
            dbhelper.DeleteDatabase();
            dbhelper.CheckAndLoadDefaults();
        }

        private void Delete_Log_Click(object sender, RoutedEventArgs e)
        {
            Log.Delete();
            Log.CreateLogFile();
        }
    }
}
