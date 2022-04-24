using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public enum State
    {        
        Aceptado,
        Pendiente,
        [Display(Name = "Rechazado por usted")]
        RechazadoPorUsted,
        Rechazado
    }
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
        public Trueque TruequeMi { get; set; }
        /// <summary>
        /// Id del trueque que recibe la solicitud
        /// </summary>
        public Trueque TruequeSu { get; set; }
        /// <summary>
        /// Id del usuario autor del trueque al que se le envió la solicitud
        /// </summary>
        public bool Si { get; set; }
        public bool No { get; set; }
        [NotMapped]
        public State State
        {
            get
            {
                if (!Si && !No)
                {
                    return State.Pendiente;
                }

                else if (Si && !No)
                {
                    return State.Aceptado;
                }

                else if (!Si && No)
                {
                    return State.RechazadoPorUsted;
                }
                else
                {
                    return State.Rechazado;
                };
            }
        }
    }
}
