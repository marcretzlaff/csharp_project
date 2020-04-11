using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace csharp_project.DataAccess
{
    public class DataManager
    {
        private DataManager() {}

        private static DataManager _instance;
        private readonly SQLiteConnection conn = new SQLiteConnection(@"Data Source=.\database.db;Version=3;Compress=True;");

        public static DataManager getInstance()
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }

        public SQLiteConnection LoadConnection()
        {
            return conn;
        }

        public void InitializeDatabase()
        {
            if (!File.Exists("./database.db"))
            {
                SQLiteConnection.CreateFile("database.db");
            }
        }
        public void CheckAndLoadDefaults()
        {
            using (SQLiteConnection dbConn = LoadConnection())
            {

            }
        }
    }
}
