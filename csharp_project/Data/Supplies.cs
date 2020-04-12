using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace csharp_project.Data
{
    abstract public class Supplies : ISupplies
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime insertTime { get { return insertTime; } set { insertTime = DateTime.Now; } }

        public DateTime expiryTime { get; set; }

        public TimeSpan lasting { get; set; }

        public bool expires { get; set; } = true;

        public string Getinformation()
        {
            return $"{Name} was inserted at {insertTime} and lasts till {expiryTime}.";
        }
    }
}
