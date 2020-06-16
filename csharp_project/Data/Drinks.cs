using SQLite;
using System;

namespace csharp_project.Data
{
    [Table("Drinks")]
    public class Drinks : Supplies
    {
        [Column("size")]
        public int volumen { get; set; } = 0;

        public Drinks() 
        {
            Name = "";
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = null;
            lasting = null;
            volumen = 0;
        }
        public Drinks(string name)
            :this()
        {
            Name = name;
        }
        public Drinks(string name, int size)
            :this(name)
        {
            volumen = size;
        }

        public Drinks(string name, DateTime inserttime, DateTime expiretime, int size)
            :this(name)
        {
            insertTime = inserttime;
            expires = true;
            expiryTime = expiretime;
            lasting = (expiretime - inserttime).Days;
            volumen = size;
        }

        public override string ToString()
        {
            return $"{Name} was inserted at {insertTime} and lasts till {expiryTime}. Volumen: {volumen}mL Expires: {expires}";
        }
    }
}
