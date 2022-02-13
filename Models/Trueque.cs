using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public enum Type
    {
        busco,
        propongo,
        completo
    }
    public class Trueque
    {        
        public int Id { get; set; }
        public int IdPerson { get; set; }
        public string Proposition { get; set; }
        public string Search { get; set; }
        public string ExtraInfo { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Date { get; set; }
        /// <summary>
        /// Type of exchange:
        /// 1: "busco"
        /// 2: "propongo"
        /// 3: "busco y propongo"
        /// </summary>
        public Type Type { get; set; }        

    }
}
