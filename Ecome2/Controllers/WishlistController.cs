using Microsoft.AspNetCore.Mvc;
using Ecome2.Models;
using Ecome2.EXtentions;
using Microsoft.EntityFrameworkCore;
using Ecome2.DAL;


namespace Ecome2.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext appDbContext;
        public WishlistController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
            };

            var twoModels = new TwoModels
            {
                CartModel = cartVM
            };

            return View(twoModels);
        }

        [HttpPost]
        public IActionResult AddToWisList(int id)
        {
            Products product = appDbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Title,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrlBase = product.ImgUrlBase
                });
            }

            HttpContext.Session.SetJson("Cart", cart);

            // Calculate total prices
            decimal grandTotal = cart.Sum(item => item.Total);
            decimal subTotal = cart.Sum(item => item.Quantity * item.Price);
            decimal Total = cart.Sum(item => item.Quantity * item.Price);

            return Json(new { success = true, grandTotal, subTotal, Total });
        }

    }
}
