using System;
using SQLite;

namespace csharp_project.Data
{
    [Table("Food")]
    public class Food : Supplies
    {
        #region Public Constructors

        public Food()
        {
            Name = "";
            InsertTime = DateTime.Now;
            Expires = false;
            ExpiryTime = null;
            Lasting = null;
            Weigth = 0;
        }

        public Food(string name) : this()
        {
            Name = name;
            InsertTime = DateTime.Now;
        }

        public Food(string name, int size) : this(name)
        {
            Weigth = size;
        }

        public Food(string name, DateTime inserttime, DateTime expiretime, int size) : this(name)
        {
            InsertTime = inserttime;
            Expires = true;
            ExpiryTime = expiretime;
            Lasting = (expiretime - inserttime).Days;
            Weigth = size;
        }

        #endregion Public Constructors

        #region Public Properties

        [Column("size")]
        public int Weigth { get; set; } = 0;

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name} was inserted at {InsertTime} and lasts till {ExpiryTime}. Weight: {Weigth}g Expires: {Expires}";
        }

        #endregion Public Methods
    }
}
