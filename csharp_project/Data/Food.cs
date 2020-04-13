using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace csharp_project.Data
{
    [Table("Food")]
    public class Food : Supplies
    {
        private int weigth { get; set; }
        public Food() { }
        public Food(string name)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = DateTime.Now;
            lasting = 0;
            weigth = 0;
        }

        public Food(string name, int seize)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = DateTime.Now;
            lasting = 0;
            weigth = seize;
        }

        public Food(string name, DateTime inserttime, DateTime expiretime, int seize)
        {
            Name = name;
            insertTime = inserttime;
            expires = true;
            expiryTime = expiretime;
            lasting = (expiretime - inserttime).Days;
            weigth = seize;
        }

        public override string ToString()
        {
            return $"{Name} was inserted at {insertTime} and lasts till {expiryTime}. Weight: {weigth}g Expires: {expires}";
        }
    }
}
