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
    }
}
