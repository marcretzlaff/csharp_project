using SQLite;
using System;

namespace csharp_project.Data
{
    [Table("Drinks")]
    public class Drinks : Supplies
    {
        #region Public Constructors

        public Drinks()
        {
            Name = "";
            InsertTime = DateTime.Now;
            Expires = false;
            ExpiryTime = null;
            Lasting = null;
            Volumen = 0;
        }

        public Drinks(string name) : this()
        {
            Name = name;
        }

        public Drinks(string name, int size) : this(name)
        {
            Volumen = size;
        }

        public Drinks(string name, DateTime inserttime, DateTime expiretime, int size) : this(name)
        {
            InsertTime = inserttime;
            Expires = true;
            ExpiryTime = expiretime;
            Lasting = (expiretime - inserttime).Days;
            Volumen = size;
        }

        #endregion Public Constructors

        #region Public Properties

        [Column("size")]
        public int Volumen { get; set; } = 0;

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"{Name} was inserted at {InsertTime} and lasts till {ExpiryTime}. Volumen: {Volumen}mL Expires: {Expires}";
        }

        #endregion Public Methods
    }
}
