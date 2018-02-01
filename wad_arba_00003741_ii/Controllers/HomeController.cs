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
        public ActionResult Index()
        {
            return View();
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

            }
            return View(new ProductViewModel());
        } 
    }
}