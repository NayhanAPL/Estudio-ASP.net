using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using versión_5_asp.Data;
using versión_5_asp.Models;

namespace versión_5_asp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [Display(Name = "Correo electrónico")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        Provincia P { get; set; }
        Municipio M { get; set; }   
        public class InputModel
        {
        
            [Display(Name = "Nombre")]
            public string FName { get; set; }

            [Display(Name = "Apellidos")]
            public string LName { get; set; }

            [Phone]
            [Display(Name = "Número Móvil")]
            public string PhoneNumber { get; set; }

            [Phone]
            [Display(Name = "Número Fijo")]
            public string Landline { get; set; }

            [EmailAddress]
            [Display(Name = "Correo Electrónico")]
            public string Email { get; set; }

            
            [Display(Name = "Dirección")]
            public string Address { get; set; }

            [Display(Name = "Provincia")]
            public List<SelectListItem> Provinces{ get; set; }
            [Display(Name = "Municipio")]
            public List<SelectListItem> Municipalities { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FName = user.FirstName,
                LName = user.LastName,
                Address = user.Address,
                Landline = user.Landline,
                Email = email,

                Provinces = new List<SelectListItem>(),
                Municipalities = new List<SelectListItem>()

            };

            Username = userName;
            P = user.Province;
            M = user.Municipality;

           await _context.Provincias.ForEachAsync(a =>
           Input.Provinces.Add(
           new SelectListItem()
           {
               Value = a.Id.ToString(),
               Text = a.Name,
               Selected = user.Province != null && a.Name == user.Province.Name
           }));


           await _context.Municipios.ForEachAsync(a =>
           Input.Municipalities.Add(
            ( new SelectListItem() { 
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = user.Municipality != null && a.Name == user.Municipality.Name
            })));

            
            ViewData["ps"] = Input.Provinces;
            ViewData["ms"] = Input.Municipalities;
        }

        public async Task<IActionResult> OnGetAsync()
        {       
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.Include(u => u.Municipality).Include(u => u.Province)
                                                   .Where(u => u.Id == currentUserId)
                                                     .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound($"No de encontró un usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            P = user.Province;
            M = user.Municipality;

            await LoadAsync(user);
            return Page();
            }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se encontró el ususario con ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var fName = Input.FName;
            var lName = Input.LName;
            var address = Input.Address;
            var llandline = Input.Landline;
            var provId = ((List<SelectListItem>)ViewData["ps"]).Find(p => p.Selected).Value;
            var munId = Input.Municipalities.Find(m => m.Selected).Value;
            var email = await _userManager.GetEmailAsync(user);

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
           
                user.FirstName = fName;
                user.LastName = lName;
                user.Address = address;
                user.Landline = llandline;
                user.Province = P;
                user.Municipality = M;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
           
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Su perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}
