using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
