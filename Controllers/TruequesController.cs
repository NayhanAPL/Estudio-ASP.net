using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using versión_5_asp.Data;
using versión_5_asp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace versión_5_asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TruequesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public TruequesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IEnumerable<Trueque> Get()
        {
            return context.Trueques.ToList();
        }

        [HttpGet("{id}", Name = "truequeCreado")]
        public IActionResult GetById(int id)
        {
            var trueque = context.Trueques.FirstOrDefault(x => x.Id == id);
            if (trueque == null)
            { 
                return NotFound();
            }
            return Ok(trueque);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Trueque trueque)
        {
            if (ModelState.IsValid)
            {
                context.Trueques.Add(trueque);
                context.SaveChanges();
                return new CreatedAtRouteResult("truequeCreado", new { id = trueque.Id }, trueque);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Trueque trueque, int id)
        {
            if(trueque.Id != id)
            {
                return BadRequest();
            }
            context.Entry(trueque).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var trueque = context.Trueques.FirstOrDefault(x => x.Id == id);
            if(trueque == null)
            {
                return NotFound();
            }
            context.Trueques.Remove(trueque);
            context.SaveChanges();
            return Ok(trueque);
        }

    }
}
