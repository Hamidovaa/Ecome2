using Ecome2.DAL;
using Ecome2.EXtentions;
using Ecome2.Models;
using Ecome2.Services;
using Ecome2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<ProgramUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ProgramUser> _signInManager;
        private readonly IEmailService _emailService;
        public AccountController(AppDbContext _appDbContext, 
            UserManager<ProgramUser> userManager,
            SignInManager<ProgramUser> signInManager, 
            IEmailService emailService,
            RoleManager<IdentityRole> roleManager)
        {
            appDbContext = _appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }


      

        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            ProgramUser programUser = new ProgramUser
            {
                Email = model.Email,
                UserName = model.Email,
                Name=model.Name,
            };
            var result=await _userManager.CreateAsync(programUser, model.Password);
            if(result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(programUser);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = programUser.Id, token = token }, Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

               await _userManager.AddToRoleAsync(programUser, "User");
                await _signInManager.SignInAsync(programUser, false);

                return RedirectToAction("Index", "Home");
            }
            foreach(var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View(model);
            
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                // Email onaylama başarılı olduysa, burada gerekli işlemleri yapabilirsiniz
                return View("ConfirmEmail"); // Örneğin bir onay sayfasına yönlendirme yapılabilir
            }
            else
            {
                return RedirectToAction("Index", "Home"); // Email onaylama başarısız olursa
            }
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRemember, false);
                if (result.Succeeded)
                {
                    // Kullanıcının rollerini kontrol et
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Password or email incorrect");
                }
            }
            else
            {
                ModelState.AddModelError("", "No user found with this email");
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

     

    }
}
