using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class DashboardController : Controller
    {
        private readonly SignInManager<ProgramUser> _signInManager;

        private readonly AppDbContext appDbContext;
        public DashboardController(AppDbContext _appDbContext, SignInManager<ProgramUser> signInManager)
        {
            appDbContext = _appDbContext;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home",new { area = "null" });
        }
    }
}
