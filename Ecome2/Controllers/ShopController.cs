using Ecome2.DAL;
using Ecome2.EXtentions;
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
            .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size)
            .Where(p => p.IsActive == true &&
            p.StockQuantity > 0)
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
            var product = appDbContext.Products
            .Include(p => p.Images)
            .Include(p => p.ProductColors)
            .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductSizes)
            .ThenInclude(s => s.Size)
            .Include(p => p.Category) // Category verisini dahil et
            .FirstOrDefault(p => p.Id == id);

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
                    .ToList(),
                colors = product.ProductColors.Select(pc => pc.Color).ToList(),
                sizes=product.ProductSizes.Select(s => s.Size).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Filter(TwoModels model)
        {
            _logger.LogInformation("Selected Colors: {0}", string.Join(",", model.SelectedColors ?? new List<int>()));
            _logger.LogInformation("Selected Sizes: {0}", string.Join(",", model.SelectedSizes ?? new List<int>()));
            _logger.LogInformation("Price Range: {0} - {1}", model.MinPrice, model.MaxPrice);

            var products = appDbContext.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.StockQuantity > 0)
                .AsQueryable();

            if (model.colors != null && model.colors.Any())
            {
                products = products.Where(p => model.SelectedColors.Contains(p.Id));
            }

            if (model.sizes != null && model.sizes.Any())
            {
                products = products.Where(p => model.SelectedSizes.Contains(p.Id));
            }

            if (model.MinPrice > 0 || model.MaxPrice < 4000)
            {
                products = products.Where(p => p.Price >= model.MinPrice && p.Price <= model.MaxPrice);
            }

            model.products = await products.ToListAsync();
            model.categories = await appDbContext.Categories
                .Include(c => c.Products.Where(p => p.IsActive))
                .Where(c => c.IsActive)
                .ToListAsync();
            model.colors = await appDbContext.Colors.Where(c => c.IsActive).ToListAsync();
            model.sizes = await appDbContext.Sizes.Where(s => s.IsActive).ToListAsync();

            return View("Index", model);
        }

        // ShopController.cs
        [HttpPost]
        public IActionResult AddToCartt(int productId, int selectedColorId, int selectedSizeId, int quantity)
        {
            return RedirectToAction("AddToCartt", "Cart", new { productId, selectedColorId, selectedSizeId, quantity });
        }

      







        ////[HttpPost]
        ////public IActionResult AddToCart(int id, string color, string size)
        ////{
        ////    Products product = appDbContext.Products.Find(id);

        ////    if (product == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        ////    CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id && c.Color == color && c.Size == size);
        ////    if (cartItem != null)
        ////    {
        ////        cartItem.Quantity++;
        ////    }
        ////    else
        ////    {
        ////        cart.Add(new CartItem
        ////        {
        ////            ProductId = product.Id,
        ////            ProductName = product.Title,
        ////            Price = product.Price,
        ////            Quantity = 1,
        ////            ImageUrlBase = product.ImgUrlBase,
        ////            Color = color,
        ////            Size = size,

        ////        });
        ////    }

        ////    HttpContext.Session.SetJson("Cart", cart);

        ////    // Calculate total prices
        ////    decimal grandTotal = cart.Sum(item => item.Total);
        ////    decimal subTotal = cart.Sum(item => item.Quantity * item.Price);
        ////    decimal Total = cart.Sum(item => item.Quantity * item.Price);

        ////    return Json(new
        ////    {
        ////        success = true,
        ////        grandTotal,
        ////        subTotal,
        ////        Total,
        ////        product = new
        ////        {
        ////            product.Id,
        ////            product.Title,
        ////            product.Price,
        ////            product.ImgUrlBase
        ////        }
        ////    });
    }




}

