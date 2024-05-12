using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecome2.Controllers
{
    public class ShopController : Controller
    {

        private readonly ILogger<ShopController> _logger;

        private readonly AppDbContext appDbContext;
        public ShopController(ILogger<ShopController> logger, AppDbContext _appDbContext)
        {
            _logger = logger;
            appDbContext = _appDbContext;
        }

        public IActionResult Index()
        {
            var model = new TwoModels
            {
                categories = appDbContext.Categories
               .Include(c => c.Products.Where(p => p.IsActive == true))
               .Where(c => c.IsActive == true)
               .ToList(),
                products = appDbContext.Products.ToList()

            };
            return View(model);
        }

      
    }
}
