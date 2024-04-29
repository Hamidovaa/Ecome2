using Ecome2.Controllers;
using Ecome2.DAL;
using Ecome2.EXtentions;
using Ecome2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private  AppDbContext appDbContext;
        private  IWebHostEnvironment _env;   
        public SliderController(AppDbContext _appDbContext, IWebHostEnvironment env)
        {
           
            appDbContext = _appDbContext;
            _env= env;  
        }
        public IActionResult Index()
        {
            return View(appDbContext.Sliders.ToList());
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider )
        {
            if(!ModelState.IsValid)
            {
                return View(slider);
            }

            if (!slider.file.IsImage())
            {
                ModelState.AddModelError("photo", "Image type is not valid");
                return View(slider);
            }
            string filename = await slider.file.SaveFileAsync(_env.WebRootPath, "UploadSlider");
            slider.ImgUrl = filename;

            appDbContext.Sliders.Add(slider);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //public IActionResult Delete(int id)
        //{
        //    if(id==0) 
        //    { 
        //        return NotFound();
        //    }
        //    var slider = appDbContext.Sliders.Find(id);
        //    if(slider != null)
        //    {
        //        appDbContext.Sliders.Remove(slider);
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
            var slider = appDbContext.Sliders.Find(id);
            if (slider != null)
            {
                appDbContext.Sliders.Remove(slider);
                appDbContext.SaveChanges();
            }
            return Json(new
            {
                status = 200
            });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var model=appDbContext.Sliders.FirstOrDefault(x=>x.Id==id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> EditAsync(Slider slider)
        {
            var oldSlider = appDbContext.Sliders.Find(slider.Id);
            //if (!ModelState.IsValid)
            //{
            //    return View(slider);
            //}
            if (slider.file != null)
            {

                if (!slider.file.IsImage()) 
                {
                    ModelState.AddModelError("Photo", "Image type is not valid");
                    return View(slider);
                }
                string filename = await slider.file.SaveFileAsync(_env.WebRootPath, "UploadSlider");

                oldSlider.ImgUrl = filename;
            }
            oldSlider.SubTitle = slider.SubTitle;
            oldSlider.Title = slider.Title;
            oldSlider.Price = slider.Price;
            oldSlider.IsCheck = slider.IsCheck;


            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
