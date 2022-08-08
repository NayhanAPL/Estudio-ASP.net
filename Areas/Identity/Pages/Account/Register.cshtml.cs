using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using versión_5_asp.Data;
using versión_5_asp.Models;

namespace versión_5_asp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Display(Name = "Nombre")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Apellidos")]
            public string LastName { get; set; }


            [Display(Name = "Dirección")]
            public string Address { get; set; }

            [Display(Name = "Provincia")]
            [BindProperty]
            public int ChosenProvId { get; set; }
            public List<SelectListItem> Provinces { get; set; }
            [Display(Name = "Municipio")]
            [BindProperty]
            public int ChosenMunId { get; set; }
            public List<SelectListItem> Municipalities { get; set; }


            [Display(Name = "No. Celular")]
            public string Cellphone { get; set; }

            [Display(Name = "No. Fijo")]
            public string Landline { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Correo Electrónico")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y como máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la confirmación de la contraseña no coinciden.")]
            public string ConfirmationPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            Input = new InputModel() { Provinces = new List<SelectListItem>(),
                                        Municipalities = new List<SelectListItem>()};

            await _context.Provincias.ForEachAsync(a =>
            Input.Provinces.Add(
            new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                
            }));           

            await _context.Municipios.ForEachAsync(a =>
            Input.Municipalities.Add(
              new SelectListItem()
              {
                  Value = a.Id.ToString(),
                  Text = a.Name,
                
              }));

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser { 
                                    UserName = Input.Email,
                                    Email = Input.Email,
                                    FirstName = Input.FirstName,
                                    LastName = Input.LastName,
                                    Address = Input.Address,
                                    Landline = Input.Landline,
                                    ProvinceId = Input.ChosenProvId,
                                    MunicipalityId = Input.ChosenMunId };

                var result = await _userManager.CreateAsync(appUser, Input.Password);
                await _userManager.SetPhoneNumberAsync(appUser, Input.Cellphone);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = appUser.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirme su correo electrónico",
                    //    $"Por favor, de click <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aquí</a> para confirmar la creación de su cuenta");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(appUser, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
