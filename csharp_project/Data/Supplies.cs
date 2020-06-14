using System;
using SQLite;

namespace csharp_project.Data
{
    public abstract class Supplies : ISupplies
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime insertTime { get;  set; }

        public DateTime? expiryTime { get; set; }

        public int? lasting { get; set; }

        public bool expires { get; set; } = true;

        public string GetInformation()
        {
            return $"{Name} was inserted at {insertTime}. ";
        }
    }
}
