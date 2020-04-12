using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using csharp_project.Data;
using MyLog;

namespace csharp_project.DataAccess
{
    public class DataManager
    {
        private DataManager() {}

        private readonly string path = "./database.db";
        private static DataManager _instance;
        private SQLiteConnection conn;

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
                conn = new SQLiteConnection(path);
        }
        public void CheckAndLoadDefaults()
        {
            using (var dbconn = LoadConnection())
            {
                dbconn.CreateTable<Food>();
                dbconn.CreateTable<Drinks>();
            }
        }

        public bool Insert<T>(ref T data)
        {
            using( var dbconn = LoadConnection())
            {
                dbconn.CreateTable<T>(); //creates table if not exists
                if (dbconn.Insert(data) != 0)
                {
                    Log.WriteLog($"{DateTime.Now} Insert: {data.ToString()}");
                    return true;
                }
            }
            return false;
        }

        public T Get<T>(int primarykey) where T : new()
        {
            using (var dbconn = LoadConnection())
            {
                var re = dbconn.Get<T>(primarykey);
                return re;
            }
        }

        public List<T> GetTable<T>() where T : new()
        {
            using (var dbconn = LoadConnection())
            {
                var re = dbconn.Table<T>().ToList();
                return re;
            }
        }
    }
}
