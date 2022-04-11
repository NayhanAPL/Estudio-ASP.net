using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Trueque TruequeMi{ get; set; }
        public Trueque TruequeSu { get; set; }
        /// <summary>
        /// Indica la fecha en la que se completó el trueque
        /// </summary>
        public DateTime Fecha { get; set; }
    }
}
