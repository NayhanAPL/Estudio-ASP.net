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
    public class EnlaceHechoController : Controller
    {
        private readonly ApplicationDbContext context;
        public EnlaceHechoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // get all enlacesHechos 
        [HttpGet]
        public IEnumerable<EnlaceHecho> Get()
        {
            return context.EnlaceHecho.ToList();
        }
        [HttpGet("{id}", Name = "enlaceHechoCreado")]
        public IActionResult GetById(int id)
        {
            var enlaceHecho = context.EnlaceHecho.FirstOrDefault(x => x.Id == id);
            if (enlaceHecho == null)
            {
                return NotFound();
            }
            return Ok(enlaceHecho);
        }
        [HttpPost]
        public IActionResult Post([FromBody] EnlaceHecho enlaceHecho)
        {
            if (ModelState.IsValid)
            {
                context.EnlaceHecho.Add(enlaceHecho);
                context.SaveChanges();
                return new CreatedAtRouteResult("enlaceCreado", new { id = enlaceHecho.Id }, enlaceHecho);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] EnlaceHecho enlaceHecho, int id)
        {
            if (enlaceHecho.Id != id)
            {
                return BadRequest();
            }
            context.Entry(enlaceHecho).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var enlaceHecho = context.EnlaceHecho.FirstOrDefault(x => x.Id == id);
            if (enlaceHecho == null) return BadRequest();
            context.EnlaceHecho.Remove(enlaceHecho);
            context.SaveChanges();
            return Ok(enlaceHecho);
        }
    }
}
