using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecome2.Controllers
{
    public class MyOrderController : Controller
    {
        private readonly AppDbContext appDbContext;
        public MyOrderController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = appDbContext.OrderItems
                .Include(o => o.Order)
                    .Include(o=> o.Product)
                .Where(o => o.Order.UserId == userId) 
                .ToList();


            return View(orders); // Doğru view dosyasını döndürdüğümüzden emin olun
        }

    }
}
