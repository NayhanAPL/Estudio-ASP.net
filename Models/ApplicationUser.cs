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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }    
        public string Address { get; set; }
        public string Landline { get; set; }
        public int ProvinceId { get; set; }
        public Provincia Province { get; set; }
        public int MunicipalityId { get; set; }
        public Municipio Municipality { get; set; }    
        public List<Trueque> Trueques { get; set; }

    }
}
