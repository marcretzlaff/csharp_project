using SQLite;
using System;

namespace csharp_project.Data
{
    [Table("Drinks")]
    public class Drinks : Supplies
    {
        [Column("size")]
        public int volumen { get; set; } = 0;

        public Drinks() { }
        public Drinks(string name)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = null;
            lasting = null;
            volumen = 0;
        }
        public Drinks(string name, int size)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = null;
            lasting = null;
            volumen = size;
        }

        public Drinks(string name, DateTime inserttime, DateTime expiretime, int size)
        {
            Name = name;
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
