using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }
        public int TruequeId { get; set; }
    }
}
