using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecome2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext appDbContext;
        public HomeController(ILogger<HomeController> logger, AppDbContext _appDbContext)
        {
            _logger = logger;
            appDbContext = _appDbContext;
        }

        public IActionResult Index()
        {

            var model = new TwoModels
            {
                sliders = appDbContext.Sliders.Where(x => x.IsCheck != false).ToList(),
                categories = appDbContext.Categories
                .Include(c => c.Products.Where(p => p.IsActive == true))
                .Where(c => c.IsActive==true)
                .ToList()
                
            };
            return View(model);
        }
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var productsByCategory = appDbContext.Products.Where(p => p.CategoryId == categoryId && p.IsActive == true).ToList();
            return Json(productsByCategory);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
