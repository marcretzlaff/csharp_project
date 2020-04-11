using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_project.Data
{
    public class Drinks : Supplies
    {
        public bool expires = true;

        private int volumen;

        public bool DoesExpire()
        {
            return expires;
        }
    }
}
