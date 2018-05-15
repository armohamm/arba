using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using wad_arba_00003741_ii.Helper;
using wad_arba_00003741_ii.Models;

namespace wad_arba_00003741_ii.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        #region Product

        [HttpGet]
        public ActionResult Index(string category, 
                                  string name, 
                                  SortCriteria? criteria, 
                                  SortOrder? order,
                                  int? page)
        {
            var model = new ProductsViewModel
            {
                Category = category,
                Name = name,
                Criteria = criteria ?? SortCriteria.Name,
                Order = order ?? SortOrder.ASC
            };
            var products = new ProductRepo().GetAll();
            
            model.Categories = products.Select(p => p.Category)
                                                                .Distinct()
                                                                .Select(c => new SelectListItem { Value = c, Text = c })
                                                                .ToList();
            model.Categories.Add(new SelectListItem { Value = "", Text = "Select category" });

            ViewBag.Categories = model.Categories;

            ViewBag.Name = name ?? "";
            ViewBag.Criteria = criteria ?? SortCriteria.Name;
            ViewBag.Order = order ?? SortOrder.ASC;
            ViewBag.Category = category ?? "";


            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category.Equals(category));

            if (!string.IsNullOrEmpty(name))
                products = products.Where(p => p.Name.ToLower().Contains(name.ToLower()) || p.Category.ToLower().Contains(name.ToLower()));

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

            LogHelper.log(this);

            model.Products = products.Select(MapToProductsModel).ToList();

            return View(model.Products.ToPagedList(page ?? 1, 10));
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            LogHelper.log(this);

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

                LogHelper.log(this);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            new ProductRepo().Delete(Id);

            LogHelper.log(this);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ProductViewModel model = MapToModel(new ProductRepo().GetById(Id));

            LogHelper.log(this);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = MapFromModel(model);
                var repo = new ProductRepo();
                repo.Update(product);

                LogHelper.log(this);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        private ProductViewModel MapToModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                IsActive = product.Available
            };
        }

        private ProductsViewModel MapToProductsModel(Product product)
        {
            return new ProductsViewModel
            {
                Id = product.Id,
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
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Category = model.Category,
                Available = model.IsActive
            };
        }

        #endregion

        #region Import

        [HttpGet]
        public ActionResult ImportProduct()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ImportProduct(FormCollection formCollection)
        {
            HttpPostedFileBase file = Request.Files["fileToImport"];

            if(file == null)
            {
                ViewBag.Result = "File is missing";
                return View();
            }

            var products = convertFileContentToProduct(file);
            saveProductsToDb(products);
            ViewBag.Products = products;

            return View();
        }

        private List<ProductImportModel> convertFileContentToProduct(HttpPostedFileBase file)
        {
            var products = new List<ProductImportModel>();
            var productsError = new List<string>();
            int errorNumber = 0;
            int productNumber = 0;

            using (var reader = new StreamReader(file.InputStream))
            {
                string line;
                int lineNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    var tokens = line.Split('|');
                    string errorMessage = "Exception in line " + lineNumber + "";
                    
                    if(tokens.Length == 4)
                    {
                        var product = new ProductImportModel();

                        errorMessage += " Wrong value given to ";
                        bool isValid = true;

                        if (!string.IsNullOrEmpty(tokens[0]))
                        {
                            product.Name = tokens[0];
                        }
                        else
                        {
                            errorMessage += "'Name' ";
                            errorNumber++;
                            isValid = false;
                        }

                        if (!string.IsNullOrEmpty(tokens[1]))
                        {
                            product.Category = tokens[1];
                        }
                        else
                        {
                            errorMessage += "'Category' ";
                            errorNumber++;
                            isValid = false;
                        }

                        decimal price;

                        if (Decimal.TryParse(tokens[2], out price))
                        {
                            product.Price = price;
                        }
                        else
                        {
                            errorMessage += "'Price' ";
                            errorNumber++;
                            isValid = false;
                        }

                        bool available;

                        if (Boolean.TryParse(tokens[3], out available))
                        {
                            product.Available = available;
                        }
                        else
                        {
                            errorMessage += "'Available' ";
                            errorNumber++;
                            isValid = false;
                        }

                        if (isValid)
                        {
                            products.Add(product);
                            productNumber++;
                        }
                    }
                    else
                    {
                        errorMessage += ": Number of parameters does not match with required parameters";
                        productsError.Add(errorMessage);
                        errorNumber++;
                    }
                }
            }

            ViewBag.NumOfProducts = productNumber;
            ViewBag.Products = products;
            ViewBag.NumOfErrors = errorNumber;
            ViewBag.productsError = productsError;

            return products;
        }

        private void saveProductsToDb(List<ProductImportModel> products)
        {
            ProductRepo repo = new ProductRepo();

            foreach (ProductImportModel product in products)
            {
                Product p = new Product()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    Available = product.Available
                };

                LogHelper.log(this, p);

                repo.Create(p);
            }

            ViewBag.Products = products;
        }

        #endregion

        #region Log
        [HttpGet]
        public ActionResult Logs()
        {
            return View();
        }

        #endregion

        #region Populate
        [HttpGet]
        public ActionResult Populate()
        {
            List<Product> products = new List<Product>();

            Product p1 = new Product()
            {
                Name = "Honor 7X",
                Price = 189,
                Category = "Smartphone",
                Available = true
            };
            products.Add(p1);

            Product p2 = new Product()
            {
                Name = "BLU Advance",
                Price = 59,
                Category = "Smartphone",
                Available = true
            };
            products.Add(p2);

            Product p3 = new Product()
            {
                Name = "Samsung Galaxy J3",
                Price = 64,
                Category = "Smartphone",
                Available = true
            };
            products.Add(p3);

            Product p4 = new Product()
            {
                Name = "Lecco Le S3",
                Price = 138,
                Category = "Smartphone",
                Available = true
            };
            products.Add(p4);

            Product p5 = new Product()
            {
                Name = "DOOGEE BL5000",
                Price = 169,
                Category = "Smartphone",
                Available = true
            };
            products.Add(p5);

            Product l1 = new Product()
            {
                Name = "Fire HD 8",
                Price = 59,
                Category = "Tablet",
                Available = true
            };
            products.Add(l1);

            Product l2 = new Product()
            {
                Name = "Fire 7",
                Price = 39,
                Category = "Tablet",
                Available = true
            };
            products.Add(l2);

            Product l3 = new Product()
            {
                Name = "Panasonic Eluga",
                Price = 62,
                Category = "Tablet",
                Available = true
            };
            products.Add(l3);

            Product l4 = new Product()
            {
                Name = "Samsung Galaxy Tab E",
                Price = 179,
                Category = "Tablet",
                Available = true
            };
            products.Add(l4);

            Product l5 = new Product()
            {
                Name = "Fire 7 Kids Edition",
                Price = 79,
                Category = "Tablet",
                Available = true
            };
            products.Add(l5);

            Product t1 = new Product()
            {
                Name = "Acer Aspire E 15",
                Price = 350,
                Category = "Laptop",
                Available = true
            };
            products.Add(t1);

            Product t2 = new Product()
            {
                Name = "HP Envy",
                Price = 420,
                Category = "Laptop",
                Available = true
            };
            products.Add(t2);

            Product t3 = new Product()
            {
                Name = "HP Pavilion Power",
                Price = 320,
                Category = "Laptop",
                Available = true
            };
            products.Add(t3);

            Product t4 = new Product()
            {
                Name = "HP 17z",
                Price = 389,
                Category = "Laptop",
                Available = true
            };
            products.Add(t4);

            Product t5 = new Product()
            {
                Name = "OMEN",
                Price = 720,
                Category = "Laptop",
                Available = true
            };
            products.Add(t5);

            Product pc1 = new Product()
            {
                Name = "HP 8000",
                Price = 99,
                Category = "PC",
                Available = true
            };
            products.Add(pc1);

            Product pc2 = new Product()
            {
                Name = "HP 8300",
                Price = 201,
                Category = "PC",
                Available = true
            };
            products.Add(pc2);

            Product pc3 = new Product()
            {
                Name = "Acer Aspire Desktop",
                Price = 499,
                Category = "PC",
                Available = true
            };
            products.Add(pc3);

            Product pc4 = new Product()
            {
                Name = "Cyberpower Gamer Extreme",
                Price = 749,
                Category = "PC",
                Available = true
            };
            products.Add(pc4);

            Product pc5 = new Product()
            {
                Name = "SkyTech ArchAngel",
                Price = 679,
                Category = "PC",
                Available = true
            };
            products.Add(p5);

            var repo = new ProductRepo();
            repo.CreateAll(products);

            return View();
        }

        #endregion
    }
}