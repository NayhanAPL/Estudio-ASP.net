using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using versión_5_asp.Data;
using versión_5_asp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace versión_5_asp.Controllers
{

    public class TruequesWebController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TruequesWebController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        //get all trueques
        //[Route("todos-los-trueques")]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter,
    string searchString,
    int? pageNumber)
        {
            //string currentUserId = "";
            var trueques = from t in _context.Trueques
                           select t;

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                string currentUserId = claim.Value;
                trueques = trueques.Where(t => t.ApplicationUserId != currentUserId);
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                trueques = trueques.Where(t => t.Proposition.Contains(searchString)
                                       || t.Search.Contains(searchString) || t.ExtraInfo.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "prop_desc":
                    trueques = trueques.OrderByDescending(s => s.Proposition);
                    break;
                case "Date":
                    trueques = trueques.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    trueques = trueques.OrderByDescending(s => s.Date);
                    break;
                default:
                    trueques = trueques.OrderBy(s => s.Proposition);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Trueque>.CreateAsync(trueques.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        //get all trueques from certain user
        //[Route("trueques/user/{id}")]//this action is attribute-routed
        public async Task<IActionResult> GetTruequesFromCurrentUser()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!String.IsNullOrWhiteSpace(currentUserId))
            {
                var elems = await _context.Trueques.OrderBy(t => t.Date).Where(x => x.ApplicationUserId == currentUserId).ToListAsync();
                return View("MisTrueques", elems);
            }
            return BadRequest("User not uthentiated");
        }

        //get all trueques of certain province and not current user
        //[Route("trueques/province/{province}")]
        public async Task<IActionResult> GetTruequesFromMyProvince()
        {
            var trueques = new List<Trueque>();
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //en caso de que sea solo la provincia del ususario
            var user = await _context.Users.Include(u => u.Province)
                                                     .Where(u => u.Id == currentUserId)
                                                       .FirstOrDefaultAsync();

            //JOIN
            var ts = await _context.Trueques.ToListAsync();
            var us = await _context.Users.Include(u => u.Province).ToListAsync();
            var res = ts.Join(us, t => t.ApplicationUserId, u => u.Id, (t, u) => (t, u))
                .Where(tuple => tuple.t.ApplicationUserId != currentUserId && tuple.u.Province.Id == user.Province.Id).ToList();
            res.ForEach(x =>
            {
                x.t.ApplicationUser = x.u;
                trueques.Add(x.t);
            });

            //var res = await _context.Users
            //    .Include(x => x.Trueques)
            //    .Where(x => x.Id != currentUserId && x.Province.Name == user.Province.Name).ToListAsync();
            //res.ForEach(x => trueques.AddRange(x.Trueques));

            return View(nameof(Index), trueques);
        }

        //get al trueques which serach field matches with a certain proposal
        [Route("trueques/match-my-proposal/{proposal}")]
        public async Task<IActionResult> GetBuscoPorPropuesta(string proposal)
        {
            var trueques = new List<Trueque>();
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.Include(u => u.Province)
                                                     .Where(u => u.Id == currentUserId)
                                                       .FirstOrDefaultAsync();

            //JOIN
            var ts = await _context.Trueques.ToListAsync();
            var us = await _context.Users.Include(u => u.Province).ToListAsync();
            var res = ts.Join(us, t => t.ApplicationUserId, u => u.Id, (t, u) => (t, u))
                .Where(tuple => tuple.t.ApplicationUserId != currentUserId &&
                    tuple.u.Province.Id == user.Province.Id &&
                        tuple.t.Search.Contains(proposal)).ToList();
            res.ForEach(x => trueques.Add(x.t));

            //var res = await _context.Users
            //    .Include(x => x.Trueques)
            //    .Where(x => x.Id != currentUserId && x.Province.Id == user.Province.Id).ToListAsync();
            //res.ForEach(x => trueques.AddRange(x.Trueques));

            return View(nameof(Index), trueques);
        }

        //get al trueques which proposal field matches with a certain search
        [Route("trueques/match-my-search/{search}")]
        public async Task<IActionResult> GetPropuestaPorBusqueda(string search)
        {
            var trueques = new List<Trueque>();
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.Include(u => u.Province)
                                                     .Where(u => u.Id == currentUserId)
                                                       .FirstOrDefaultAsync();

            //JOIN
            var ts = await _context.Trueques.ToListAsync();
            var us = await _context.Users.Include(u => u.Province).ToListAsync();
            var res = ts.Join(us, t => t.ApplicationUserId, u => u.Id, (t, u) => (t, u))
                .Where(tuple => tuple.t.ApplicationUserId != currentUserId &&
                    tuple.u.Province.Id == user.Province.Id &&
                        tuple.t.Proposition.Contains(search)).ToList();
            res.ForEach(x => trueques.Add(x.t));

            //var res = await _context.Users
            //    .Include(x => x.Trueques)
            //    .Where(x => x.Id != currentUserId && x.Province.Id == user.Province.Id).ToListAsync();
            //res.ForEach(x => trueques.AddRange(x.Trueques));

            return View(nameof(Index), trueques);
        }

        // GET: TruequesWeb/Details/5
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();

            }

            var trueque = await _context.Trueques.FindAsync(id);
            //var user = await _context.Users.FindAsync(trueque.ApplicationUserId);
            trueque.ApplicationUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == trueque.ApplicationUserId);
            if (trueque == null)
            {
                return NotFound();
            }

            //load image
            var wwwRootPath = _hostEnvironment.WebRootPath;
            ImageModel _default = new() { ImageUrl = "sin_imagen.jpg" };
            try
            {
                var res = await _context.Imagenes.FirstAsync(x => x.TruequeId == id);
                trueque.Image = res.ImageUrl == null ? _default : res;
            }
            catch (Exception)
            {
                trueque.Image = _default;
            }
            //string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = returnUrl;
            return View(trueque);
        }

        // GET: TruequesWeb/Create
        public IActionResult Create(string returnUrl)
        {
            //string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = returnUrl;
            return View();
        }

        // POST: TruequesWeb/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,Proposition,Search,ExtraInfo,Image,Date,Type")] Trueque trueque)
        {
            if (ModelState.IsValid)
            {
                //Create Trueque
                var currentDate = DateTime.Now;
                var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                trueque.Date = currentDate;
                trueque.ApplicationUserId = currentUserID;

                //Save image to wwwroot
                if (trueque.Image != null)
                {
                    var wwwRootPath = _hostEnvironment.WebRootPath;
                    //var fileName = Path.GetFileNameWithoutExtension(trueque.Image.ImageFile.FileName);
                    var extension = Path.GetExtension(trueque.Image.ImageFile.FileName);
                    var rname = Path.GetRandomFileName().Split('.')[0];
                    trueque.Image.ImageUrl = rname + extension;//DateTime.Now.ToString("ddMMyyyyfff") + extension;
                                                               //trueque.Thumbnail.ImageUrl = rname + "_thumb." + extension;

                    var path = Path.Combine(wwwRootPath + "/Images/", rname + extension);
                    var thumbnailPath = Path.Combine(wwwRootPath + "/Images/thumbnails/", rname + "_thumb" + extension);

                    using (var memoryStream = new MemoryStream())
                    {
                        await trueque.Image.ImageFile.CopyToAsync(memoryStream);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await trueque.Image.ImageFile.CopyToAsync(fileStream);
                            var img = Image.FromStream(memoryStream);
                            // TODO: ResizeImage(img, 100, 100);
                            var thumb = img.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
                            thumb.Save(thumbnailPath);
                        };
                    };

                }
                _context.Add(trueque);
                await _context.SaveChangesAsync();
                //
                return RedirectToAction(nameof(Index));
            }
            string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = url;
            return View(trueque);
        }

        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var img = Image.FromStream(memoryStream))
                {
                    // TODO: ResizeImage(img, 100, 100);
                    return Ok();
                }
            }
        }
        // GET: TruequesWeb/Edit/5
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trueque = await _context.Trueques.FindAsync(id);
            if (trueque == null)
            {
                return NotFound();
            }
            //load image
            var wwwRootPath = _hostEnvironment.WebRootPath;
            ImageModel _default = new() { ImageUrl = "sin_imagen.jpg" };
            try
            {
                var res = await _context.Imagenes.FirstAsync(x => x.TruequeId == id);
                trueque.Image = res.ImageUrl == null ? _default : res;
            }
            catch (Exception)
            {
                trueque.Image = _default;
            }
            //string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = returnUrl;
            return View(trueque);
        }

        // POST: TruequesWeb/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,Proposition,Search,ExtraInfo,Date,Type,Image")] Trueque trueque, string returnUrl)
        {
            if (id != trueque.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trueque);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TruequeExists(trueque.Id))
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
            //string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = returnUrl;
            return View(trueque);
        }

        // GET: TruequesWeb/Delete/5
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trueque = await _context.Trueques
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trueque == null)
            {
                return NotFound();
            }
            //load image
            var wwwRootPath = _hostEnvironment.WebRootPath;
            ImageModel _default = new() { ImageUrl = "sin_imagen.jpg" };
            try
            {
                var res = await _context.Imagenes.FirstAsync(x => x.TruequeId == id);
                trueque.Image = res.ImageUrl == null ? _default : res;
            }
            catch (Exception)
            {
                trueque.Image = _default;
            }
            string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = returnUrl;
            return View(trueque);
        }

        // POST: TruequesWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trueque = await _context.Trueques.FindAsync(id);
            ImageModel image = new();
            //load image url
            if (await _context.Imagenes.AnyAsync(x => x.TruequeId == id))
            {
                image = await _context.Imagenes.FirstOrDefaultAsync(x => x.TruequeId == id);
                //delete image     
                var splitRes = image.ImageUrl.Split(".");
                var filePath = Path.Combine(_hostEnvironment.WebRootPath + "/Images/" + image.ImageUrl);
                var thumbPath = Path.Combine(_hostEnvironment.WebRootPath + "/Images/thumbnails/" + splitRes[0] + "_thumb." + splitRes[1]);
                //delete img
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                //delete thumbnail
                if (System.IO.File.Exists(thumbPath))
                {
                    System.IO.File.Delete(thumbPath);
                }
                //delete records
                _context.Imagenes.Remove(image);
            }

            if (!_context.Enlace.Any(e => e.TruequeMi.Id == id || e.TruequeSu.Id == id))
            {
                _context.Trueques.Remove(trueque);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(GetTruequesFromCurrentUser));
        }

        private bool TruequeExists(int id)
        {
            return _context.Trueques.Any(e => e.Id == id);
        }


        public async Task<PartialViewResult> SearchTrueques(string searchText)
        {
            if (searchText == null) { searchText = ""; }
            var res = new List<Trueque>();
            if (searchText != "")
            {
                var toLlower = searchText.ToLower();
                //res = await _context.Trueques.Where(t => t.Proposition.Contains(searchText.ToLower())
                //                    || t.Search.Contains(searchText)
                //                        || t.ExtraInfo.Contains(searchText)).ToListAsync();
                var users_trueques = await _context.Users.Include(u => u.Trueques).ToListAsync();
                foreach (var user in users_trueques)
                {
                    user.Trueques.ForEach(t => t.ApplicationUser = user);
                    res.AddRange(user.Trueques.Where(t => t.Proposition.ToLower().Contains(searchText) || t.Search.ToLower().Contains(searchText)
                                          || t.ExtraInfo.ToLower().Contains(searchText)).ToList());
                }
                //res = _context.Trueques.Include(t => t.ApplicationUser).Where(t => t.Proposition.Contains(searchText) || t.Search.Contains(searchText)
                //|| t.ExtraInfo.Contains(searchText)).ToList();
                //await users_trueques.ForEachAsync(u => res.AddRange(u.Trueques.Where(t => t.Proposition.Contains(searchText) || t.Search.Contains(searchText)
                //                          || t.ExtraInfo.Contains(searchText)).ToList()));
            }
            else
            {
                res = await _context.Trueques.Include(t => t.ApplicationUser).ToListAsync();
            }

            return PartialView("_GridView", res);
        }
        public async Task<IActionResult> RequestTrueque(int id, string returnUrl)
        {
            var targetTrueque = await _context.Trueques.Include(t => t.Image).FirstOrDefaultAsync(t => t.Id == id);
            //load image
            if (targetTrueque.Image != null)
            {
                var img = await _context.Imagenes.FindAsync(targetTrueque.Image.Id);
                targetTrueque.Image = img;
            }

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                //
            }
            ViewData["PreviousUrl"] = returnUrl;
            return View("SolicitarTrueque", targetTrueque);
        }
        [HttpPost]
        public async Task<IActionResult> SendRequest([Bind("Id,ApplicationUserId,Proposition,Search,ExtraInfo,Image,Date,Type")] Trueque trueque, string truequeMiId)
        {
            var solicitudes = new List<Enlace>();
            if (ModelState.IsValid)
            {
                //Validate same trueque has not been requested
                var res = _context.Enlace.AnyAsync(e => e.TruequeMi.Id.ToString() == truequeMiId).Result;
                if (res)
                { return RedirectToAction("Index"); }

                    //Create Trueque
                    var currentDate = DateTime.Now;
                    var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    trueque.Date = currentDate;
                    trueque.ApplicationUserId = currentUserID;

                    //Save image to wwwroot
                    if (trueque.Image != null)
                    {
                        var wwwRootPath = _hostEnvironment.WebRootPath;
                        //var fileName = Path.GetFileNameWithoutExtension(trueque.Image.ImageFile.FileName);
                        var extension = Path.GetExtension(trueque.Image.ImageFile.FileName);
                        var rname = Path.GetRandomFileName().Split('.')[0];
                        trueque.Image.ImageUrl = rname + extension;//DateTime.Now.ToString("ddMMyyyyfff") + extension;
                                                                   //trueque.Thumbnail.ImageUrl = rname + "_thumb." + extension;

                        var path = Path.Combine(wwwRootPath + "/Images/", rname + extension);
                        var thumbnailPath = Path.Combine(wwwRootPath + "/Images/thumbnails/", rname + "_thumb" + extension);

                        using (var memoryStream = new MemoryStream())
                        {
                            await trueque.Image.ImageFile.CopyToAsync(memoryStream);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await trueque.Image.ImageFile.CopyToAsync(fileStream);
                                var img = Image.FromStream(memoryStream);
                                // TODO: ResizeImage(img, 100, 100);
                                var thumb = img.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
                                thumb.Save(thumbnailPath);
                            };
                        };

                    }
                    _context.Add(trueque);
                    //await _context.SaveChangesAsync();

                    //

                    //var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserID != null && truequeMiId != null)
                    {
                        var trueuqeMi = await _context.Trueques.FindAsync(int.Parse(truequeMiId));
                        var enlace = new Enlace()
                        {
                            TruequeSu = trueque,
                            TruequeMi = trueuqeMi
                        };

                        _context.Enlace.Add(enlace);
                        await _context.SaveChangesAsync();

                        solicitudes = await _context.Enlace.Include(x => x.TruequeMi).Where(x => x.TruequeSu.ApplicationUserId == currentUserID).ToListAsync();
                    
                }


            }
            return RedirectToAction("GetMisSolicitudes", "Enlaces", solicitudes);
        }
        public async Task<IActionResult> TruequeSolicitudDetails(int? id, string returnUrl, State? state)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trueque = await _context.Trueques.FindAsync(id);
            trueque.ApplicationUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == trueque.ApplicationUserId);

            if (trueque == null)
            {
                return NotFound();
            }

            //load image
            var wwwRootPath = _hostEnvironment.WebRootPath;
            ImageModel _default = new() { ImageUrl = "sin_imagen.jpg" };
            try
            {
                var res = _context.Imagenes.FirstOrDefault(x => x.TruequeId == id);
                if (trueque.Image != null) { trueque.Image = res.ImageUrl == null ? _default : res; }
                else
                {
                    trueque.Image = _default;
                }
            }
            catch (Exception)
            {
                trueque.Image = _default;
            }
            string url = Request.Headers["Referer"].ToString();
            ViewData["PreviousUrl"] = returnUrl;
            if (state != null) { ViewData["State"] = state.ToString(); }           
            return View(trueque);
        }

        public async Task<IActionResult> Profile(string uId, string returnUrl)
        {
            //var trueque = await _context.Trueques.FirstOrDefaultAsync(t=>t.Id == tId);
            var user = await _context.Users.FindAsync(uId);
            ViewData["PreviousUrl"] = returnUrl;
            return View(user);
        }
    }
}
