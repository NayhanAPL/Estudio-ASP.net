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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }    
        public string Address { get; set; }
        public string Landline { get; set; }
        public Municipio Municipality { get; set; }
        public Provincia Province { get; set; }
    }
}
