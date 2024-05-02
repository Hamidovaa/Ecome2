using Ecome2.DAL;
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
        public AccountController(AppDbContext _appDbContext, UserManager<ProgramUser> userManager)
        {
            appDbContext = _appDbContext;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(LoginVM model)
        {
            if(ModelState.IsValid)
            {
                return View(model);
            }
            ProgramUser programUser = new ProgramUser
            {
                Email = model.Email,
                UserName=model.Email
            };
            var result=await _userManager.CreateAsync(programUser, model.Password);
            if(result.Succeeded)
            {
                RedirectToAction("Index", "Home");
            }
            return View();
            
        }
    }
}
