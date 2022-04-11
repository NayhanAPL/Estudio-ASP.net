using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public class Provincia
    {
        public int Id{ get; set; }
        public string Name{ get; set; }
        public List<Municipio> Municipalities { get; set; }
    }
}
