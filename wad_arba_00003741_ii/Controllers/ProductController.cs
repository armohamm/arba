using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace wad_arba_00003741_rest_ii.Controllers
{
    public class ProductController : ApiController
    {
        public List<Product> GetString()
        {
            List<Product> products = new ProductRepo().GetAll().ToList();

            return products;

        }

        [HttpGet]
        public string AddProduct(string name
            , decimal price
            , string category
            , bool active
            )
        {
            ProductRepo repo = new ProductRepo();
            Product product = new Product
            {
                Name = name,
                Price = price,
                Category = category,
                Available = active         
            };

            repo.Create(product);

            return "Product added - name: " + name + " price: " + price + " category: " + category + " active: " + active;
        } 

        //public string GetString()
        //{
        //    return "Hello world";
        //}

    }
}
