using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    /// <summary>
    /// Clase que representa una asociación entre dos trueques
    /// </summary>
    public class Enlace
    {
        /// <summary>
        /// Id del enlace
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id del trueque al que se le envió la solicitud
        /// </summary>
        public int IdMi { get; set; }
        /// <summary>
        /// Id del trueque que recibe la solicitud
        /// </summary>
        public int IdSu { get; set; }
        /// <summary>
        /// Id del usuario autor del trueque al que se le envió la solicitud
        /// </summary>
        public int IdPersonaMi { get; set; }
        /// <summary>
        /// Id del usuario autor del trueque que se envió como solicitud
        /// </summary>
        public int IdPersonaSu { get; set; }
        public bool Si { get; set; }
        public bool No { get; set; }
    }
}
