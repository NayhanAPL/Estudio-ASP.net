using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using versión_5_asp.Models;

namespace versión_5_asp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
<<<<<<< HEAD
        public DbSet<Enlace> enlace { get; set; }
        public DbSet<EnlaceHecho> enlaceHecho { get; set; }
=======

        public DbSet<Trueque>Trueques { get; set; }
>>>>>>> 8cbf75546c61da2cae843107de67d76d247750de
    }
}
