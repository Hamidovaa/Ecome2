using Ecome2.Controllers;
using Ecome2.DAL;
using Ecome2.EXtentions;
using Ecome2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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

        //public IActionResult Search(string searchText)
        //{
        //    // Arama metnini kullanarak ürünleri filtreleyin
        //    var filteredProducts = appDbContext.Products
        //        .Where(p => p.Title.ToLower().Contains(searchText.ToLower()))
        //        .Select(p => new Products
        //        {
        //            Title = p.Title,
        //            ImgUrlBase = p.ImgUrlBase
        //        })
        //        .ToList();

        //    // JSON formatında filtrelenmiş ürünleri geri döndürün
        //    return Json(filteredProducts);
        //}


        public JsonResult Activate(int id)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 400
                });
            }
            var products = appDbContext.Products.Find(id);
            if (products == null)
            {
                return Json(new
                {
                    status = 400
                });
            }
            products.IsActive = !products.IsActive;  // Kategoriyi aktif hale getir
            appDbContext.SaveChanges();
            return Json(new
            {
                status = 200
            });
        }

        //public JsonResult Delete(int id)
        //{
        //    if (id == 0)
        //    {
        //        return Json(new
        //        {
        //            status = 400
        //        });
        //    }
        //    var products = appDbContext.Products.Find(id);
        //    if (products != null)
        //    {
        //        appDbContext.Products.Remove(products);
        //        appDbContext.SaveChanges();
        //    }
        //    return Json(new
        //    {
        //        status = 200
        //    });
        //}

        public IActionResult Create()
        {
            ViewBag.Category = appDbContext.Categories.ToList();
            ViewBag.Color=appDbContext.Colors.ToList();
            ViewBag.Size=appDbContext.Sizes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Category = appDbContext.Categories.ToList();
                ViewBag.Color = appDbContext.Colors.ToList();
                ViewBag.Size=appDbContext.Sizes.ToList() ;
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
            if (model.file != null)
            {
                foreach (var item in model.ColorsId)
                {
                    ProductColor productColor = new ProductColor
                    {
                        ProductId = model.Id,
                        ColorId =item
                    };
                    appDbContext.ProductColors.Add(productColor);
                    appDbContext.SaveChanges();
                }
            }
            if (model.file != null)
            {
                foreach (var i in model.ProductSizes)
                {
                    ProductSize productSize = new  ProductSize
                    {
                        ProductId = model.Id,
                        SizeId = i,
                    };
                    appDbContext.ProductSizes.Add(productSize);
                    appDbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

          
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Category = appDbContext.Categories.ToList();
            ViewBag.Color = appDbContext.Colors.ToList();
            ViewBag.Size = appDbContext.Sizes.ToList();
            var model = appDbContext.Products.Include(x => x.ProductColors).Include(x => x.Images).FirstOrDefault(x => x.Id == id);
            var dbColors=appDbContext.ProductColors.Where(x=>x.ProductId == id).ToList();
            var dbSizes=appDbContext.ProductSizes.Where(x=>x.ProductId == id).ToList();
            model.ColorsId = new List<int>();
            model.ProductSizes=new List<ProductSize>();
            foreach(var item in dbColors)
            {
                model.ColorsId.Add(item.ColorId);
            }
            foreach (var item in dbSizes)
            {
                model.ColorsId.Add(item.SizeId);
            }
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
            ViewBag.Color = appDbContext.Colors.ToList();
            ViewBag.Size = appDbContext.Sizes.ToList();
            var oldProducts = appDbContext.Products.FirstOrDefault(x=>x.Id==products.Id);

            oldProducts.Title = products.Title;
            oldProducts.Description = products.Description;
            oldProducts.Price = products.Price;
            oldProducts.CategoryId = products.CategoryId;
            oldProducts.ColorsId = products.ColorsId;
            oldProducts.ProductSizes = products.ProductSizes;
            appDbContext.SaveChanges();
            //if (!ModelState.IsValid)
            //{
            //    return View(slider);
            //}
            if (products.ColorsId == null)
            {
                return RedirectToAction("Edit");
            }
            if (products.ProductSizes == null)
            {
                return RedirectToAction("Edit");
            }
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
            var colorsDb = appDbContext.ProductColors.Where(x => x.ProductId == products.Id);
                appDbContext.ProductColors.RemoveRange(colorsDb);
                appDbContext.SaveChanges();
            var sizesDb = appDbContext.ProductSizes.Where(x => x.ProductId == products.Id);
            appDbContext.ProductSizes.RemoveRange(sizesDb);
            appDbContext.SaveChanges();
            foreach (var item in products.ColorsId)
            {
                ProductColor productColor = new ProductColor
                {
                    ProductId = products.Id,
                    ColorId = item
                };
                appDbContext.ProductColors.Add(productColor);
                appDbContext.SaveChanges();
            }
            foreach (var i in products.ProductSizes)
            {
                ProductSize productSize = new ProductSize
                {
                    ProductId = products.Id,
                    SizeId = i
                };
                appDbContext.ProductSizes.Add(productSize);
                appDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public JsonResult DeleteImages(int id)
        {
            if (id != 0)
            {
                var model = appDbContext.Images.Find(id);
                appDbContext.Images.Remove(model);
                appDbContext.SaveChanges();
            }
            return Json(new
            {
                status = 200
            });
        }
        

    }
}
