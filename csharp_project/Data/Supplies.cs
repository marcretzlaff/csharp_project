using System;
using SQLite;

namespace csharp_project.Data
{
    public abstract class Supplies : ISupplies
    {
        #region Public Properties

        public bool Expires { get; set; } = true;

        public DateTime? ExpiryTime { get; set; }

        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        public DateTime InsertTime { get; set; }

        public int? Lasting { get; set; }

        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public string GetInformation()
        {
            return $"{Name} was inserted at {InsertTime}. ";
        }

        #endregion Public Methods
    }
}
