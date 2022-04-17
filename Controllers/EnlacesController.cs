using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using versión_5_asp.Data;
using versión_5_asp.Models;

namespace versión_5_asp.Controllers
{
    public class EnlacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnlacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Enlaces
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enlace.Include(x=>x.TruequeMi).Include(x=>x.TruequeSu).ToListAsync());
        }
        public async Task<IActionResult> GetMisSolicitudes()
        {
            var res = new List<Enlace>();
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var currentUserID = claim.Value;
                res = await _context.Enlace.Include(e=>e.TruequeSu).Include(e => e.TruequeMi).Where(e => e.TruequeSu.ApplicationUserId == currentUserID).ToListAsync();            
            }
            return View("SolicitudesEnviadas", res);
        }
        public async Task<IActionResult> GetSolicitudesRecibidas()
        {
            var res = new List<Enlace>();
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var currentUserID = claim.Value;
                res = await _context.Enlace.Include(e => e.TruequeSu).Include(e => e.TruequeMi).Where(e => e.TruequeMi.ApplicationUserId == currentUserID).ToListAsync();
            }
            return View("SolicitudesRecibidas", res);
        }
        // GET: Enlaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enlace = await _context.Enlace
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enlace == null)
            {
                return NotFound();
            }

            return View(enlace);
        }

        // GET: Enlaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enlaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Si,No")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enlace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enlace);
        }

        // GET: Enlaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enlace = await _context.Enlace.FindAsync(id);
            if (enlace == null)
            {
                return NotFound();
            }
            string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = url;
            return View(enlace);
        }

        // POST: Enlaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Si,No")] Enlace enlace)
        {
            if (id != enlace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnlaceExists(enlace.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
         
            return View(enlace);
        }

        // GET: Enlaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enlace = await _context.Enlace
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enlace == null)
            {
                return NotFound();
            }

            return View(enlace);
        }

        // POST: Enlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enlace = await _context.Enlace.FindAsync(id);
            _context.Enlace.Remove(enlace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnlaceExists(int id)
        {
            return _context.Enlace.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Aceptar(int id)
        {
            //var query = from enlace in _context.Enlace
            //            where enlace.Id == id
            //            select enlace;

            var enlace = await _context.Enlace.FirstOrDefaultAsync(e => e.Id == id);
            enlace.Si = true;
            await _context.SaveChangesAsync();

            string url = Request.Headers["Referer"].ToString();
            return Redirect(url);
        }
        public async Task<IActionResult> Rechazar(int id)
        {
            var enlace = await _context.Enlace.FirstOrDefaultAsync(e => e.Id == id);
            enlace.No = true;
            await _context.SaveChangesAsync();

            string url = Request.Headers["Referer"].ToString();
            return Redirect(url);
        }
        public async Task<IActionResult> FinalizarTrueque(int id)
        {
            var enlace = await _context.Enlace.Include(e=>e.TruequeMi).Include(e=>e.TruequeSu).FirstOrDefaultAsync(e=>e.Id == id);
            if (enlace!=null)
            {
                //crear entrada en tabla EnlaceHecho
                await _context.EnlaceHecho.AddAsync(new EnlaceHecho()
                {
                    TruequeMi = enlace.TruequeMi,
                    TruequeSu = enlace.TruequeSu,
                    Fecha = DateTime.Now
                });
                _context.Enlace.Remove(enlace);
            }
            await _context.SaveChangesAsync();      
            
            string url = Request.Headers["Referer"].ToString();
            return Redirect(url);
        }
    }
}
