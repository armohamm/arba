using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wad_arba_00003741_ii.Models;

namespace wad_arba_00003741_ii.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            var repo = new ProductRepo();
            var products = repo.GetAll();
            return View(products.Select(MapToModel));
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View(new ProductViewModel());
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = MapFromModel(model);
                var repo = new ProductRepo();
                repo.Create(product);
                return RedirectToAction("Index");
            }
            return View(new ProductViewModel());
        }

        private ProductViewModel MapToModel(Product product)
        {
            return new ProductViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                IsActive = product.Available
            };
        }
        
        private Product MapFromModel(ProductViewModel model)
        {
            return new Product
            {
                Name = model.Name,
                Price = model.Price,
                Category = model.Category,
                Available = model.IsActive
            };
        }

    }
}