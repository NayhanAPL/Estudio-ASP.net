using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using versión_5_asp.Models;

namespace versión_5_asp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Enlace> Enlace { get; set; }
        public DbSet<EnlaceHecho> EnlaceHecho { get; set; }
        public DbSet<Trueque> Trueques { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<ImageModel> Imagenes { get; set; }
    }
}
