using Microsoft.AspNetCore.Mvc;
using Ecome2.Models;
using Ecome2.EXtentions;
using Microsoft.EntityFrameworkCore;
using Ecome2.DAL;

namespace Ecome2.Controllers
{
    public class CartController : Controller
    {

        private readonly AppDbContext appDbContext;
        public CartController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            var twoModels = new TwoModels
            {
                CartModel = cartVM
            };

            return View(twoModels);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
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


        [HttpPost]
        public async Task<IActionResult> Increase(int id)
        {
            var product = await appDbContext.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Title,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            // Calculate total prices
            decimal grandTotal = cart.Sum(item => item.Total);
            decimal subTotal = cart.Sum(item => item.Quantity * item.Price);
            decimal itemTotal = cartItem.Quantity * cartItem.Price;

            return Json(new { success = true, grandTotal, subTotal, itemTotal});
        }

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem != null && cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            // Calculate total prices
            decimal grandTotal = cart.Sum(item => item.Total);
            decimal subTotal = cart.Sum(item => item.Quantity * item.Price);
            decimal itemTotal = cartItem.Quantity * cartItem.Price;

            return Json(new { success = true, grandTotal, subTotal,itemTotal });
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(new { status = 400 });
            }

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                HttpContext.Session.SetJson("Cart", cart); // Update session

                // Calculate total prices
                decimal grandTotal = cart.Sum(item => item.Total);
                decimal subTotal = cart.Sum(item => item.Quantity * item.Price);
                decimal itemTotal = cartItem.Quantity * cartItem.Price;
                int itemCount = cart.Count;

                return Json(new { success = true, grandTotal, subTotal, itemTotal, itemCount });
            }

            return Json(new { status = 404 });
        }
    }
}
