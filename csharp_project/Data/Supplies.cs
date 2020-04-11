using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_project.Data
{
    abstract public class Supplies : ISupplies
    {
        public string Name;

        protected DateTime insertTime = DateTime.Now;

        protected DateTime expiryTime;

        protected TimeSpan lasting;


        public string Getinformation()
        {
            return $"{Name} was inserted at {insertTime} and lasts till {expiryTime}.";
        }

        public DateTime GetInsertDate()
        {
            return insertTime;
        }

        public DateTime GetExpiryDate()
        {
            return expiryTime;
        }
    }
}
