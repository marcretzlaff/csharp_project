using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using csharp_project.Data;
using MyLog;
using System.Windows.Forms;

namespace csharp_project.DataAccess
{
    public class DataManager
    {

        private readonly string path = Application.StartupPath + "/database.db";

        #region Singelton
        private DataManager() { }
        private static DataManager _instance;

        /// <summary>
        /// Singelton implementation
        /// </summary>
        /// <returns></returns>
        public static DataManager getInstance()
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }
        #endregion Singleton

        /// <summary>
        /// returns activ Connection
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(path,true);
        }

        /// <summary>
        /// Deletes Item tables and reloads with default
        /// </summary>
        public void DeleteDatabase()
        {
            using (var dbconn = GetConnection())
            {
                dbconn.DropTable<Food>();
                dbconn.DropTable<Drinks>();
            }

            CheckAndLoadDefaults();
        }

        /// <summary>
        /// Default Table Creation
        /// </summary>
        public void CheckAndLoadDefaults()
        {
            using (var dbconn = GetConnection())
            {
                dbconn.CreateTable<Food>();
                dbconn.CreateTable<Drinks>();
            }
        }

        /// <summary>
        /// Inserts Item in corresponding table
        /// </summary>
        /// <typeparam name="T">Itemtyp</typeparam>
        /// <param name="data">Item</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets item from table with corresponding ID
        /// </summary>
        /// <typeparam name="T">Itemtyp</typeparam>
        /// <param name="primarykey">ID of Item</param>
        /// <returns></returns>
        public T Get<T>(int primarykey) where T : new()
        {
            using (SQLiteConnection dbconn = GetConnection())
            {
                var re = dbconn.Get<T>(primarykey);
                return re;
            }
        }

        /// <summary>
        /// Deletes item from table with corresponding ID
        /// </summary>
        /// <typeparam name="T">Itemtyp</typeparam>
        /// <param name="primarykey">Id of Item to delete</param>
        /// <returns></returns>
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

        /// <summary>
        /// Loads Table from database
        /// </summary>
        /// <typeparam name="T">Itemtyp</typeparam>
        /// <returns></returns>
        public List<T> GetTable<T>() where T : new()
        {
            using (SQLiteConnection dbconn = GetConnection())
            {
                var re = dbconn.Table<T>();
                return re.ToList();
            }
        }

        /// <summary>
        /// Returns List with Items corresponding to name search
        /// </summary>
        /// <typeparam name="T">Itemtyp</typeparam>
        /// <param name="name">search string</param>
        /// <returns></returns>
        public List<T> Get<T>(string name) where T : Supplies, new()
        {
            using (SQLiteConnection dbconn = GetConnection())
            {
                return (from i in dbconn.Table<T>() where i.Name.ToLower() == name.ToLower() select i).ToList();
            }
        }

        /// <summary>
        /// Returns List of Items which expire in supplied month
        /// </summary>
        /// <typeparam name="T">Itemtyp</typeparam>
        /// <param name="month">Month</param>
        /// <returns></returns>
        public List<T> Get<T>(DateTime month) where T : Supplies, new()
        {
            using (SQLiteConnection dbconn = GetConnection())
            {
                var list = (from i in dbconn.Table<T>() where i.expires == true select i).ToList();
                return list.Where(x => x.expiryTime.Value.Month == month.Month).ToList();
            }
        }

        /// <summary>
        /// Updates corresponding Item
        /// </summary>
        /// <typeparam name="T">Itemtyp</typeparam>
        /// <param name="data">item to update</param>
        /// <returns></returns>
        public bool Update<T>(T data) where T : Supplies, new()
        {
            using (var dbconn = GetConnection())
            {
                if (0 != dbconn.Update(data))
                {
                    Log.WriteLog($"Updated: {data.ToString()}");
                    return true;
                }
                else
                {
                    Log.WriteLog($"Update of Item failed: {data.ToString()}");
                }
                return false;
            }
        }
    }
}
