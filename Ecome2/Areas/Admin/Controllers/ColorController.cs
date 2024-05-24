using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext appDbContext;
        public ColorController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public IActionResult Index()
        {
            var colors = appDbContext.Colors.ToList();
            return View(colors);
        }


        [HttpPost]
        public IActionResult Create(Color color)
        {
            if (ModelState.IsValid)
            {
                return View(color);
            }
            appDbContext.Colors.Add(color);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult Activate(int id)
        {
            if (id == 0 )
            {
                return Json(new
                {
                    status = 400
                });
            }
            var color = appDbContext.Colors.Find(id);
            if (color == null)
            {
                return Json(new
                {
                    status = 400
                });
            }
            color.IsActive = !color.IsActive;  
            appDbContext.SaveChanges();
            return Json(new
            {
                status = 200
            });
        }




        [HttpGet]
        public JsonResult Edit(int id)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 400
                });
            }
            var color = appDbContext.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null)
            {
                return Json(new
                {
                    status = 400
                });
            }

            return Json(color);
        }

        [HttpPost]
        public IActionResult Edit(Color color)
        {

            if (!ModelState.IsValid)
            {
                return View(color);
            }
            appDbContext.Colors.Update(color);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
