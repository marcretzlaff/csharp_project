using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using csharp_project.Data;
using MyLog;
using System.Windows.Forms;
using Unity;

namespace csharp_project.DataAccess
{
    public class DataManager :IDatabase
    {
        #region Private Fields

        private readonly string path = Application.CommonAppDataPath + "\\database.db";

        #endregion Private Fields

        #region Public Constructors

        public DataManager(UnityContainer container) { Container = container; }

        #endregion Public Constructors

        #region Public Properties

        //public Container due InjectionProperty UnityContainer
        public UnityContainer Container { get; set; }

        #endregion Public Properties

        #region Public Methods

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
                    Container.Resolve<Log>().WriteLog($"Deleted: {temp}");
                    return true;
                }
                else
                {
                    Container.Resolve<Log>().WriteLog($"Delete of Item failed: {temp}");
                }

                return false;
            }
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
                var list = (from i in dbconn.Table<T>() where i.Expires == true select i).ToList();
                return list.Where(x => x.ExpiryTime.Value.Month == month.Month).ToList();
            }
        }

        /// <summary>
        /// returns activ Connection
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection GetConnection()
        {
            SQLiteConnection con = null;

            try
            {
                con = new SQLiteConnection(path, true);
            }
            catch (Exception e)
            {
                Container.Resolve<Log>().WriteException(e, "Connection String Loading");
            }

            return con;
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
                    Container.Resolve<Log>().WriteLog($"Insert: {data}");
                    return true;
                }
            }

            return false;
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
                    Container.Resolve<Log>().WriteLog($"Updated: {data}");
                    return true;
                }
                else
                {
                    Container.Resolve<Log>().WriteLog($"Update of Item failed: {data}");
                }

                return false;
            }
        }

        #endregion Public Methods
    }
}
