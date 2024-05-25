using Ecome2.DAL;
using Ecome2.Models;
using Ecome2.EXtentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Microsoft.AspNetCore.Authorization;
using Stripe.Climate;
using System.Linq;
using Stripe;
using Microsoft.EntityFrameworkCore;

namespace Ecome2.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<ProgramUser> _userManager;

        public CheckoutController(AppDbContext _appDbContext, UserManager<ProgramUser> userManager)
        {
            appDbContext = _appDbContext;
            _userManager = userManager;
        }

        private static Models.Order tempOrder;
        private static decimal total = 0;

        [HttpPost]
        public IActionResult Checkout(Models.Order order)
        {
            var list = HttpContext.Session.GetJson<List<CartItem>>("Cart");
        
            if (list == null)
            {
                return RedirectToAction("ShopCart", "Cart");
            }

            tempOrder = order;
            var domain = "https://localhost:7085/";
            var options = new SessionCreateOptions()
            {
                SuccessUrl = domain + "CheckOut/OrderConfirmation",
                CancelUrl = domain + "CheckOut/Payment",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };
            foreach (var item in list)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = item.Price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName
                        }
                    },
                    Quantity = item.Quantity
                };
                total += item.Total;
                options.LineItems.Add(sessionListItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            TempData["Session"] = session.Id;
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet]
        public IActionResult Payment()
        {
            ViewBag.StripePublishableKey = "your_publishable_key";
            return View();
        }

        //public IActionResult Success(int orderId)
        //{
        //    var order = appDbContext.Orders
        //        .Include(x => x.OrderItems)
        //        .ThenInclude(x => x.Product)
        //        .FirstOrDefault(x => x.Id == orderId);

        //    if (order == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(order);
        //}


        [Authorize]
        [HttpGet]
        public IActionResult Invoice()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return View("Failed");
            }


            var cartList = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            return View();
        }


        public async Task<IActionResult> OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View("Fail");
                }

                var random = new Random();
                tempOrder.UserId = user.Id;
                tempOrder.OrderNumber = random.Next(100000, 1000000);
                tempOrder.Date = DateTime.Now;
                tempOrder.Total = total;


                appDbContext.Orders.Add(tempOrder);
                appDbContext.SaveChanges();

                var cartList = HttpContext.Session.GetJson<List<CartItem>>("Cart");
                foreach (var item in cartList)
                {
                    var orderDetail = new OrderItem
                    {
                        OrderId = tempOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Subtotal = item.Total,
                        Color = item.Color,
                        Size = item.Size
                    };
                    appDbContext.OrderItems.Add(orderDetail);

                    var product = appDbContext.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    product.StockQuantity -= item.Quantity;
                }
                appDbContext.SaveChanges();

                var _order = appDbContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == tempOrder.Id);
                return View("Success", _order);
            }
            return View("Fail");
        }
    }
}
