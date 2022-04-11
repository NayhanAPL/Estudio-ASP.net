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
    [Route("api/provincias/{ProvinceId}/municipios")]
    public class MunicipiosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MunicipiosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(int ProvinceId)
        {
            var mun = _context.Municipios.Where(x=>x.Province.Id== ProvinceId).ToList();
            return Ok(mun);
        }

        [HttpGet("{id}", Name = "municipioCreado")]
        public IActionResult GetById(int provinceID, int id)
        {
            var mun = _context.Municipios.FirstOrDefault(x => (x.Province.Id == provinceID && x.Id == id));
            //province.Municipalities = _context.Municipios.Where(x => x.Province.Id == province.Id).ToList();
            if (mun == null)
            {
                return NotFound();
            }
            return Ok(mun);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Municipio mun, int ProvinceId)
        {
            if (ModelState.IsValid)
            {
                mun.Province.Id = ProvinceId;
                _context.Municipios.Add(mun);
                _context.SaveChanges();
                //return new CreatedAtRouteResult("municipioCreado", new { id = mun.Id }, mun);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Municipio mun, int id)
        {
            if (mun.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(mun).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var mun = _context.Municipios.FirstOrDefault(x => x.Id == id);
            if (mun == null)
            {
                return NotFound();
            }
            _context.Municipios.Remove(mun);
            _context.SaveChanges();
            return Ok(mun);
        }
    }

}
