using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    /// <summary>
    /// Registro de trueques completados
    /// </summary>
    public class EnlaceHecho
    {
        public int Id { get; set; }
        public int IdMi { get; set; }
        public int IdSu { get; set; }
        public int IdPersonaMi { get; set; }
        public int IdPersonaSu { get; set; }
        /// <summary>
        /// Indica la fecha en la que se completó el trueque
        /// </summary>
        public DateTime Fecha { get; set; }
    }
}
