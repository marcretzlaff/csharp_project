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
        private int volumen;

        public override string ToString()
        {
            return $"{Name} was inserted at {insertTime} and lasts till {expiryTime}. Volumen: {volumen}mL Expires: {expires}";
        }
    }
}
