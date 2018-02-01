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
        [HttpGet]
        public ActionResult Index(string category, 
                                  string name, 
                                  SortCriteria? criteria, 
                                  SortOrder? order)
        {
            var model = new ProductsViewModel
            {
                Category = category,
                Name = name,
                Criteria = criteria ?? SortCriteria.Name,
                Order = order ?? SortOrder.ASC
            };

            var products = new ProductRepo().GetAll();
            model.Categories = products.Select(p => p.Category).Distinct().Select(c => new SelectListItem { Value = c, Text = c}).ToList();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category.Equals(category));

            if (!string.IsNullOrEmpty(name))
                products = products.Where(p => p.Name.ToLower().Contains(name.ToLower()));

            if(criteria == SortCriteria.Price)
            {
                if(order == SortOrder.DESC)
                    products = products.OrderByDescending(p => p.Price);
                else
                    products = products.OrderBy(p => p.Price);
            }
            else
            {
                if (order == SortOrder.DESC)
                    products = products.OrderByDescending(p => p.Name);
                else
                    products = products.OrderBy(p => p.Name);
            }

            model.Products = products.Select(MapToModel).ToList();

            return View(model);
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