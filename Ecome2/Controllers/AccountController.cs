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
                await _signInManager.SignInAsync(programUser, true);

                return RedirectToAction("Index", "Home");
            }
            foreach(var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View();
            
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginVM model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");  
            }
            var user =await _userManager.FindByEmailAsync(model.Email);
            if (user!=null)
            { 
               var result= await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRemember, false );
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Password or email incorrect");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task SeedRoles()
        {
           if(!await _roleManager.RoleExistsAsync(roleName: "Admin"))
           {
                await _roleManager.CreateAsync(new IdentityRole(roleName: "Admin"));
           }
           if (!await _roleManager.RoleExistsAsync(roleName: "User"))
           {
                await _roleManager.CreateAsync(new IdentityRole(roleName: "User"));
           }
        }

        public async Task SeedAdmin()
        {
            if (_userManager.FindByEmailAsync("hemidoffa55@gmail.com").Result == null)
            {
                ProgramUser programUser = new ProgramUser
                {
                    Email = "hemidoffa55@gmail.com",
                    UserName = "hemidoffa55@gmail.com",
                    Name = "",
                };
                var result =await _userManager.CreateAsync(programUser, "Ecome2");
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(programUser, "Admin");
                    await _signInManager.SignInAsync(programUser, true);

                    RedirectToAction ("Index", "Home");

                }
            }
        }
    }
}
