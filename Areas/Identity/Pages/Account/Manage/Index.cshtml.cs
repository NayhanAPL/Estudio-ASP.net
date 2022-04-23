using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Nombre")]
            public string FName { get; set; }

            [Display(Name = "Apellidos")]
            public string LName { get; set; }

            [Phone]
            [Display(Name = "Número Móvil")]
            public string PhoneNumber { get; set; }

            [EmailAddress]
            [Display(Name = "Correo Electrónico")]
            public string Email { get; set; }

            
            [Display(Name = "Dirección")]
            public string Address { get; set; }
         
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Username = userName;
            
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FName = user.FirstName,
                LName = user.LastName,
                Address = user.Address,
                Email = email
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No de encontró un usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
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

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
           
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Su perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}
