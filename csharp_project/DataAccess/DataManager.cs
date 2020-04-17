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

        public static DataManager getInstance()
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(path,true);
        }

        public void DeleteDatabase()
        {
            using (var dbconn = GetConnection())
            {
                dbconn.DropTable<Food>();
                dbconn.DropTable<Drinks>();
            }

            CheckAndLoadDefaults();
        }
        public void CheckAndLoadDefaults()
        {
            using (var dbconn = GetConnection())
            {
                dbconn.CreateTable<Food>();
                dbconn.CreateTable<Drinks>();
            }
        }

        public bool Insert<T>(T data) where T: new()
        {
            using( var dbconn = GetConnection())
            {
                dbconn.CreateTable<T>(); //creates table if not exists
                if (dbconn.Insert(data) != 0)
                {
                    Log.WriteLog($"Insert: {data.ToString()}");
                    return true;
                }
            }
            return false;
        }

        public T Get<T>(int primarykey) where T : new()
        {
            using (SQLiteConnection dbconn = GetConnection())
            {
                var re = dbconn.Get<T>(primarykey);
                return re;
            }
        }

        public bool Delete<T>(int primarykey) where T : new()
        {
            using (var dbconn = GetConnection())
            {
                var temp = Get<T>(primarykey);
                if (0 != dbconn.Delete<T>(primarykey))
                {
                    Log.WriteLog($"Deleted: {temp.ToString()}");
                    return true;
                }
                else
                {
                    Log.WriteLog($"Delete of Item failed: {temp.ToString()}");
                }
                return false;
            }
        }
        public List<T> GetTable<T>() where T : new()
        {
            using (SQLiteConnection dbconn = GetConnection())
            {
                var re = dbconn.Table<T>();
                return re.ToList();
            }
        }

        public List<T> Get<T>(string name) where T : Supplies, new()
        {
            using (SQLiteConnection dbconn = GetConnection())
            {
                return (from i in dbconn.Table<T>() where i.Name.ToLower() == name.ToLower() select i).ToList();
            }
        }
    }
}
