using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public class ApplicationUser: IdentityUser
    {
        //Propiedad Navigacional
        public List<Trueque> Trueques{ get; set; }
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [NotMapped]
        [Display(Name = "Nombre")]
        public string FullName { get { return FirstName + " " + LastName; } }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Municipio")]
        public Municipio Municipality { get; set; }
        [Display(Name = "Provincia")]
        public Provincia Province { get; set; }
    }
}
