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

        public IActionResult Create()
        {
            ViewBag.Category = appDbContext.Categories.ToList();
            return View();
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
            model.ImgUrlBase = filename;
            appDbContext.Products.Add(model);
            appDbContext.SaveChanges();

            if(model.file != null)
            {
                foreach (var item in model.ImagesFiles)
                {
                    if (!item.IsImage())
                    {
                        ModelState.AddModelError("photo", "Image type is not valid");
                        return View(model);
                    }
                    string filename2 = await item.SaveFileAsync(_env.WebRootPath, "UploadProducts");

                    Images images = new Images
                    {
                        ProductId = model.Id,
                        ImgUrl = filename2
                    };
                    appDbContext.Images.Add(images);
                    appDbContext.SaveChanges();
                }
            }  
            return RedirectToAction("Index");
        }

          
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Category = appDbContext.Categories.ToList();
            var model = appDbContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == id);

            if (id == 0)
            {
                return NotFound();
            }
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
            var oldProducts = appDbContext.Products.FirstOrDefault(x=>x.Id==products.Id);

            oldProducts.Title = products.Title;
            oldProducts.Description = products.Description;
            oldProducts.Price = products.Price;
            oldProducts.IsCheck = products.IsCheck;
            oldProducts.CategoryId = products.CategoryId;
            appDbContext.SaveChanges();
            //if (!ModelState.IsValid)
            //{
            //    return View(slider);
            //}

            if (oldProducts == null)
            {
                return RedirectToAction("Index");
            }
            if(products.file != null)
            {
                if (!products.file.IsImage())
                {
                    ModelState.AddModelError("photo", "Image type is not valid");
                    return View(products);
                }
                string filename = await products.file.SaveFileAsync(_env.WebRootPath, "UploadProducts");

                oldProducts.ImgUrlBase = filename;
                appDbContext.SaveChanges();
            }
            if (products.ImagesFiles != null)
            {
                foreach(var item in products.ImagesFiles)
                {
                    if (!item.IsImage()) 
                    {
                        ModelState.AddModelError("photo", "Image type is not valid");
                        return View(products);
                    }
                    string filename2 = await item.SaveFileAsync(_env.WebRootPath, "UploadProducts");

                    Images images = new Images
                    {
                        ProductId = products.Id,
                        ImgUrl = filename2
                    };
                    appDbContext.Images.Add(images);
                    appDbContext.SaveChanges();


                }   
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteImages(int id)
        {
            if (id != 0)
            {
                var model = appDbContext.Images.Find(id);
                appDbContext.Images.Remove(model);
                appDbContext.SaveChanges();
            }
            return RedirectToAction("Edit");
        }
        

    }
}
