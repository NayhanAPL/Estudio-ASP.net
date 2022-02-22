using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using versión_5_asp.Data;
using versión_5_asp.Models;

namespace versión_5_asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnlaceController : Controller
    {
        private readonly ApplicationDbContext context;
        public EnlaceController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // get all enlaces 
        [HttpGet]
        public IEnumerable<Enlace> Get()
        {
            return context.Enlace.ToList();
        }
        [HttpGet("{id}", Name = "enlaceCreado")]
        public IActionResult GetById(int id)
        {
            var enlace = context.Enlace.FirstOrDefault(x => x.Id == id);
            if(enlace == null)
            {
                return NotFound();
            }
            return Ok(enlace);
        }
        [HttpPost]
        public IActionResult Post ([FromBody] Enlace enlace)
        {
            if(ModelState.IsValid)
            {
                context.Enlace.Add(enlace);
                context.SaveChanges();
                return new CreatedAtRouteResult("enlaceCreado", new { id = enlace.Id }, enlace);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Enlace enlace, int id )
        {
            if(enlace.Id != id)
            {
                return BadRequest();
            }
            context.Entry(enlace).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var enlace = context.Enlace.FirstOrDefault(x => x.Id == id);
            if (enlace == null) return BadRequest();
            context.Enlace.Remove(enlace);
            context.SaveChanges();
            return Ok(enlace);
        }
    }
}
