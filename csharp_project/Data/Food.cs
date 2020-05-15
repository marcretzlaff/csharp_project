using System;
using SQLite;

namespace csharp_project.Data
{
    [Table("Food")]
    public class Food : Supplies
    {
        [Column("size")]
        public int weigth { get; set; } = 0;
        public Food() { }
        public Food(string name)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = null;
            lasting = null;
            weigth = 0;
        }

        public Food(string name, int size)
        {
            Name = name;
            insertTime = DateTime.Now;
            expires = false;
            expiryTime = null;
            lasting = null;
            weigth = size;
        }

        public Food(string name, DateTime inserttime, DateTime expiretime, int size)
        {
            Name = name;
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
