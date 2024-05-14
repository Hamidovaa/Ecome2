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

        //[HttpPost]
        //public async Task<IActionResult> Index(int[] ctgIds, int[] colorIds, int[] sizeIds, int min, int max)
        //{

        //    var query = appDbContext.Products.Include(p => p.Category).Include(p => p.Images)
        //        .Include(p => p.Description)
        //        .Where(p => p.IsActive == true);

        //    if (ctgIds.Length > 0)
        //    {
        //        query = query.Where(p => ctgIds.Contains(p.CategoryId));
        //    }
        //    if (max != 0)
        //    {
        //        query = query.Where(p => (p.Price <= max && p.Price >= min));
        //    }
        //    var products = await query.ToListAsync();
        //    return PartialView("_ProductList", products);
        //}



    }
}
