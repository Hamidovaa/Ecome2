using Ecome2.Models;
using Microsoft.AspNetCore.Mvc;
using Ecome2.EXtentions;

namespace Ecome2.ViewComponents
{
    public class CartVC:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };
            return View(cartVM);
        }
    }
}
