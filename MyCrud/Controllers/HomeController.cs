using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyCrud.Models;

namespace MyCrud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeController(ApplicationContext context, IWebHostEnvironment hostEnvironment)
        {

            dbContext = context;
            webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var product = await dbContext.Products.ToListAsync();
            return View(product);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {

                string uniqueFileName = UploadedFile(model);
                Product product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    ImageName = uniqueFileName,

                };
                dbContext.Add(product);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
        
            //ProductViewModel prod1 = new ProductViewModel();
          //  prod1.Id = (int)id;


            if (id == null)
            {
                return NotFound();
            }

            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
             string uniqueFileName = UploadedFile(model);
            if (ModelState.IsValid)
            {
                Product product = new Product
                {   Id = model.Id,
                    Name = model.Name,
                    Price = model.Price,
                    ImageName = uniqueFileName,

                };
                dbContext.Update(product);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await dbContext.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await dbContext.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }




        private string UploadedFile(ProductViewModel model)
        {
            string uniqueFileName = null;

            if (model.ImageName != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageName.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageName.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private bool ProductExists(int id)
        {
            return dbContext.Products.Any(e => e.Id == id);
        }

    }
}
