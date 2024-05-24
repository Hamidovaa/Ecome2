using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SizeController : Controller
    {
        private readonly AppDbContext appDbContext;
        public SizeController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public IActionResult Index()
        {
            var sizes = appDbContext.Sizes.ToList();
            return View(sizes);
        }


        [HttpPost]
        public IActionResult Create(Size size)
        {
            if (ModelState.IsValid)
            {
                return View(size);
            }
            appDbContext.Sizes.Add(size);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult Activate(int id)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 400
                });
            }
            var size = appDbContext.Sizes.Find(id);
            if (size == null)
            {
                return Json(new
                {
                    status = 400
                });
            }
            size.IsActive = !size.IsActive;  // Kategoriyi aktif hale getir
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
            var size = appDbContext.Sizes.FirstOrDefault(x => x.Id == id);
            if (size == null)
            {
                return Json(new
                {
                    status = 400
                });
            }

            return Json(size);
        }

        [HttpPost]
        public IActionResult Edit(Size size)
        {

            if (!ModelState.IsValid)
            {
                return View(size);
            }
            appDbContext.Sizes.Update(size);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
