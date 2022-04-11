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
    [Produces("application/json")]
    [Route("api/provincias")]
    public class ProvinciasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProvinciasController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            //return Ok(_context.Provincias.ToList());
            var provincias = _context.Provincias.ToList();
            foreach (var prov in provincias)
            {
                prov.Municipalities = _context.Municipios.Where(x => x.Province.Id == prov.Id).ToList();
                
            }
            return Ok(provincias);
        }

        [HttpGet("{id}", Name = "provinciaCreada")]
        public IActionResult GetById(int id)
        {
            var province = _context.Provincias.FirstOrDefault(x => x.Id == id);
            province.Municipalities = _context.Municipios.Where(x => x.Province.Id == province.Id).ToList();
            if (province==null)
            {
                return NotFound();
            }
            return Ok(province);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Provincia province)
        {
            if (ModelState.IsValid)
            {
                _context.Provincias.Add(province);
                _context.SaveChanges();
                return new CreatedAtRouteResult("provinciaCreada", new { id = province.Id }, province);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Provincia province, int id)
        {
            if (province.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(province).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var province = _context.Provincias.FirstOrDefault(x => x.Id == id);
            if (province == null)
            {
                return NotFound();
            }
            _context.Provincias.Remove(province);
            _context.SaveChanges();
            return Ok(province);
        }
    }
}
