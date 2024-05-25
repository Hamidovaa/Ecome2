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
                products = appDbContext.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive == true && p.StockQuantity > 0)
                .ToList(),
                colors = appDbContext.Colors
                .Where(c => c.IsActive == true)
                .ToList(),
                sizes=appDbContext.Sizes.
                Where(p => p.IsActive == true)
                .ToList(),


            };
            return View(model);
        }


        // Arama metodu
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(query))
            {
                var emptyModel = new TwoModels
                {
                    products = new List<Products>()
                };
                return View(emptyModel);
            }

            var products = await appDbContext.Products
                .Where(p => p.Title.Contains(query))
                .ToListAsync();
            var model = new TwoModels
            {
                products = products
            };

            return View(model);
        }
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await appDbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            var model = new TwoModels
            {
                products = new List<Products> { product },
                categories = appDbContext.Categories
                    .Include(c => c.Products.Where(p => p.IsActive))
                    .Where(c => c.IsActive)
                    .ToList()
            };

            return View(model);
        }
    }
}
