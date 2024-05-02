using Ecome2.Controllers;
using Ecome2.DAL;
using Ecome2.EXtentions;
using Ecome2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext appDbContext;
        private IWebHostEnvironment _env;
        public ProductsController(AppDbContext _appDbContext, IWebHostEnvironment env)
        {
            appDbContext = _appDbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(appDbContext.Products.Include(x => x.Category).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Category=appDbContext.Categories.ToList();
            return View();
        }

        public JsonResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 400
                });
            }
            var products = appDbContext.Products.Find(id);
            if (products != null)
            {
                appDbContext.Products.Remove(products);
                appDbContext.SaveChanges();
            }
            return Json(new
            {
                status = 200
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Category = appDbContext.Categories.ToList();
                return View(model);
            }

            if (!model.file.IsImage())
            {
                ModelState.AddModelError("photo", "Image type is not valid");
                return View(model);
            }
            string filename = await model.file.SaveFileAsync(_env.WebRootPath, "UploadProducts");
            model.ImgUrl = filename;

            appDbContext.Products.Add(model);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Category = appDbContext.Categories.ToList();

            if (id == 0)
            {
                return NotFound();
            }
            var model = appDbContext.Products.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(Products products)
        {
            ViewBag.Category = appDbContext.Categories.ToList();
            var oldProducts = appDbContext.Products.Find(products.Id);
            //if (!ModelState.IsValid)
            //{
            //    return View(slider);
            //}
            if (products.file != null)
            {

                if (!products.file.IsImage())
                {
                    ModelState.AddModelError("Photo", "Image type is not valid");
                    return View(products);
                }
                string filename = await products.file.SaveFileAsync(_env.WebRootPath, "UploadProducts");

                oldProducts.ImgUrl = filename;
            }
            
            oldProducts.Title = products.Title;
            oldProducts.Description = products.Description;
            oldProducts.Price = products.Price;
            oldProducts.IsCheck = products.IsCheck;
            oldProducts.CategoryId = products.CategoryId;

            appDbContext.Products.Update(oldProducts);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        

    }
}
