using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using KeepFitStore.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KeepFitStore.WEB.Filters;
using KeepFitStore.Helpers;
using KeepFitStore.Models.ViewModels.Basket;
using KeepFitStore.WEB.Common;
using KeepFitStore.Services.Contracts;

namespace KeepFitStore.WEB.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [RedirectUserIfLoggedInFilter]
    public class LoginModel : PageModel
    {
        private const string ConfirmEmailMessage = "You have to confirm your email before login";
        private const string InputEmailPropAsString = "Input.Email";

        private readonly SignInManager<KeepFitUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IBasketService basketService;

        public LoginModel(SignInManager<KeepFitUser> signInManager, ILogger<LoginModel> logger, IBasketService basketService)
        {
            _signInManager = signInManager;
            _logger = logger;
            this.basketService = basketService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(this.User))
            {
                return RedirectToPage("/");
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    if (returnUrl == "/Orders/Create")
                    {
                        var basketSession = SessionHelper.GetObjectFromJson<List<BasketViewModel>>(this.HttpContext.Session, WebConstants.BasketKey);

                        var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                        var principal = await _signInManager.CreateUserPrincipalAsync(user);
                        await this.basketService.ClearBasketAsync(user.BasketId);

                        foreach (var item in basketSession)
                        {
                            await this.basketService
                                .AddProductToBasketAsync(item.Product.Id, user.UserName, item.Quantity);
                        }
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}