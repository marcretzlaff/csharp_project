using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace csharp_project.DataAccess
{
    class DataAccessHelper
    {
        private static DataAccessHelper _instance;
        private readonly SQLiteConnection conn = new SQLiteConnection(@"Data Source=.\database.db;Version=3;Compress=True;");

        public static DataAccessHelper getInstance()
        {
            if (_instance == null)
            {
                _instance = new DataAccessHelper();
            }
            return _instance;
        }

        public SQLiteConnection LoadConnection()
        {
            return conn;
        }

        public void InitializeDatabase()
        {
            if(!File.Exists("./database.db"))
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

        private DataAccessHelper() { }
    }
}
