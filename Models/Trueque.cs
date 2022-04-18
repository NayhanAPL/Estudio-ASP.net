using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public enum Type
    {
        Completo,
        Propongo,
        Busco     
    }
    public class Trueque
    {        
        public int Id { get; set; }
        [Display(Name = "Id Usuario")]
        public string ApplicationUserId{ get; set; }
        [NotMapped]
        public ApplicationUser ApplicationUser { get; set; }
        [StringLength(50)]
        [Display(Name = "Propone")]
        public string Proposition { get; set; }
        [StringLength(50)]
        [Display(Name = "Busca")]
        public string Search { get; set; }
        [StringLength(50)]//Data Annotations
        [Display(Name = "Información Extra")]
        public string ExtraInfo { get; set; }
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        [Display(Name = "Tipo de Trueque")]
        /// <summary>
        /// Type of exchange:
        /// 1: "busco"
        /// 2: "propongo"
        /// 3: "busco y propongo"
        /// </summary>
        public Type Type { get; set; }
        [Display(Name = "Foto")]
        public ImageModel Image { get; set; }//Navigational Property
    }
}
