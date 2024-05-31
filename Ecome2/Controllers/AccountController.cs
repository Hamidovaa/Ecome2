 using Ecome2.DAL;
using Ecome2.EXtentions;
using Ecome2.Models;
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



                // await _signInManager.SignInAsync(programUser, false);

                await _userManager.AddToRoleAsync(programUser, "User");

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
                // Email confirmation successful, you can redirect to a confirmation page
                // Add user to "User" role
                //await _userManager.AddToRoleAsync(user, "User");

                //// Sign in the user (optional)
                //await _signInManager.SignInAsync(user, false);

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


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["Message"] = "An email has been sent to your email address with instructions on how to reset your password.";
                return RedirectToAction("Message");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { email = email, token = token }, Request.Scheme);


            await _emailService.SendEmailAsync(email, "Reset Password", $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");
            TempData["Message"] = "An email has been sent to your email address with instructions on how to reset your password.";
            return RedirectToAction("Message");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Error");
            }

            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("Error");
                }
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "The password and confirmation password do not match.");
                    return View(model);
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Your password has been reset successfully.";
                    return RedirectToAction("Message");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Message()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

    }
}
