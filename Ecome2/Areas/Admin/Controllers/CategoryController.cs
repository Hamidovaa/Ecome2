using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext appDbContext;
        public CategoryController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public IActionResult Index()
        {
            return View(appDbContext.Categories.ToList());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                return View(category);
            }
            appDbContext.Categories.Add(category);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        //public IActionResult Delete(int id)
        //{
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var category = appDbContext.Categories.Find(id);
        //    if (category != null)
        //    {
        //        appDbContext.Categories.Remove(category);
        //        appDbContext.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        public JsonResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 400
                });
            }
            var category = appDbContext.Categories.Find(id);
            if (category != null)
            {
                appDbContext.Categories.Remove(category);
                appDbContext.SaveChanges();
            }
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
            var category = appDbContext.Categories.Find(id);
            if (category == null)
            {
                return Json(new
                {
                    status = 400
                });
            }
            return Json(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {

            if (!ModelState.IsValid)
            {
                return View(category);
            }
            appDbContext.Categories.Update(category);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
