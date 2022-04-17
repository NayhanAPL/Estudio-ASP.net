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
    public class EnlacesHechosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnlacesHechosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EnlacesHechos
        public async Task<IActionResult> Index()
        {
            return View(await _context.EnlaceHecho.ToListAsync());
        }

        // GET: EnlacesHechos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enlaceHecho = await _context.EnlaceHecho
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enlaceHecho == null)
            {
                return NotFound();
            }

            return View(enlaceHecho);
        }

        // GET: EnlacesHechos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EnlacesHechos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha")] EnlaceHecho enlaceHecho)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enlaceHecho);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enlaceHecho);
        }

        // GET: EnlacesHechos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enlaceHecho = await _context.EnlaceHecho.FindAsync(id);
            if (enlaceHecho == null)
            {
                return NotFound();
            }
            return View(enlaceHecho);
        }

        // POST: EnlacesHechos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha")] EnlaceHecho enlaceHecho)
        {
            if (id != enlaceHecho.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enlaceHecho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnlaceHechoExists(enlaceHecho.Id))
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
            return View(enlaceHecho);
        }

        // GET: EnlacesHechos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enlaceHecho = await _context.EnlaceHecho
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enlaceHecho == null)
            {
                return NotFound();
            }

            return View(enlaceHecho);
        }

        // POST: EnlacesHechos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enlaceHecho = await _context.EnlaceHecho.FindAsync(id);
            _context.EnlaceHecho.Remove(enlaceHecho);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnlaceHechoExists(int id)
        {
            return _context.EnlaceHecho.Any(e => e.Id == id);
        }
        public async Task<IActionResult> MisTruequesCompletados()
        {
            var res = new List<EnlaceHecho>();
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                res = await _context.EnlaceHecho.Include(e => e.TruequeSu)
                    .Include(e => e.TruequeMi)
                    .Where(e => e.TruequeSu.ApplicationUserId == currentUserId.Value || e.TruequeMi.ApplicationUserId == currentUserId.Value)
                    .ToListAsync();
            }
            return View(nameof(Index), res);
        }
    }  
}
