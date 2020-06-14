using System;
using System.Collections.Generic;
using csharp_project.Data;

namespace csharp_project.DataAccess
{
    public interface IDatabase
    {
        void DeleteDatabase();
        void CheckAndLoadDefaults();
        bool Insert<T>(T data) where T : new();
        T Get<T>(int primarykey) where T : new();
        bool Delete<T>(int primarykey) where T : new();
        List<T> GetTable<T>() where T : new();
        List<T> Get<T>(string name) where T : Supplies, new();
        List<T> Get<T>(DateTime month) where T : Supplies, new();
        bool Update<T>(T data) where T : Supplies, new();
    }
}
