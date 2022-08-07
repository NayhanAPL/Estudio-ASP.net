using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public class Municipio
    {
        public int Id{ get; set; }
        public string Name { get; set; }       
        [JsonIgnore]//para evitar el ciclo en la serializacion del json
        public int ProvinceId { get; set; }
        public Provincia Province { get; set; }
    }
}
