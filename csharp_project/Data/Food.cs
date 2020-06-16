using System;
using SQLite;

namespace csharp_project.Data
{
    [Table("Food")]
    public class Food : Supplies
    {
        [Column("size")]
        public int weigth { get; set; } = 0;
        public Food()
        {
            Name = "";
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = null;
            lasting = null;
            weigth = 0;
        }
        public Food(string name)
            :this()
        {
            Name = name;
            insertTime = DateTime.Now;
        }

        public Food(string name, int size)
            :this(name)
        {
            weigth = size;
        }

        public Food(string name, DateTime inserttime, DateTime expiretime, int size)
            :this(name)
        {
            insertTime = inserttime;
            expires = true;
            expiryTime = expiretime;
            lasting = (expiretime - inserttime).Days;
            weigth = size;
        }

        public override string ToString()
        {
            return $"{Name} was inserted at {insertTime} and lasts till {expiryTime}. Weight: {weigth}g Expires: {expires}";
        }
    }
}
