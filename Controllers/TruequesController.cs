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
        public IActionResult Get()
        {
            var claims = User.Claims.ToList();
            var isAdmin = claims.Any(x => x.Type == "Admin" && x.Value == "Y");
            if (isAdmin)
            {
                return Ok(context.Trueques.ToList());
            }
            var userId = claims.FirstOrDefault(x => x.Type == "id");
            
            if(userId == null)
            {
                return Unauthorized();
            }
            return Ok(context.Trueques.Where(x => x.ApplicationUserId.ToString() == userId.Value));
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
            context.Entry(trueque).State = EntityState.Modified;//modelo desconectado
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
