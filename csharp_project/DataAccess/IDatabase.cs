using System;
using System.Collections.Generic;
using csharp_project.Data;

namespace csharp_project.DataAccess
{
    public interface IDatabase
    {
        #region Public Methods

        void CheckAndLoadDefaults();

        bool Delete<T>(int primarykey) where T : new();

        void DeleteDatabase();

        T Get<T>(int primarykey) where T : new();

        List<T> Get<T>(string name) where T : Supplies, new();

        List<T> Get<T>(DateTime month) where T : Supplies, new();

        List<T> GetTable<T>() where T : new();

        bool Insert<T>(T data) where T : new();

        bool Update<T>(T data) where T : Supplies, new();

        #endregion Public Methods
    }
}
