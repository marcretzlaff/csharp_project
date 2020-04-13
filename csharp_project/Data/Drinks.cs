using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_project.Data
{
    [Table("Drinks")]
    public class Drinks : Supplies
    {
        private int volumen { get; set; } = 0;

        public Drinks() { }
        public Drinks(string name)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = DateTime.Now;
            lasting = 0;
            volumen = 0;
        }
        public Drinks(string name, int seize)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = DateTime.Now;
            lasting = 0;
            volumen = seize;
        }

        public Drinks(string name, DateTime inserttime, DateTime expiretime, int seize)
        {
            Name = name;
            insertTime = inserttime;
            expires = true;
            expiryTime = expiretime;
            lasting = (expiretime - inserttime).Days;
            volumen = seize;
        }

        public override string ToString()
        {
            return $"{Name} was inserted at {insertTime} and lasts till {expiryTime}. Volumen: {volumen}mL Expires: {expires}";
        }
    }
}
