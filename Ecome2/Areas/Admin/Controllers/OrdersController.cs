using Ecome2.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext appDbContext;
        private IWebHostEnvironment _env;

        public OrdersController(AppDbContext _appDbContext, IWebHostEnvironment env)
        {
            appDbContext = _appDbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            var orders = appDbContext.Orders
           .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Product)
           .OrderByDescending(o => o.Date) // Tarihe göre sıralama, en yeni tarihler en son
           .ToList();

            return View(orders);
        }
    }
}
