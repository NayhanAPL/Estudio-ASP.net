using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}
