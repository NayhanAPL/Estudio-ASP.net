using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Trueque Propuesto")]
        public Trueque TruequeMi{ get; set; }
        [Display(Name = "Trueque Solicitado")]
        public Trueque TruequeSu { get; set; }
        /// <summary>
        /// Indica la fecha en la que se completó el trueque
        /// </summary>
        [Display(Name = "Fecha de conclusión")]
        public DateTime Fecha { get; set; }
    }
}
