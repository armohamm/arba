using ArbaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbaShop.DAL.Repos
{
    class ProductRepo
    {
        private ArbaShopDbEntities db;

        public ProductRepo()
        {
            db = new ArbaShopDbEntities();
        }

        public IQueryable<Product> GetAll()
        {
            return db.Products;
        }

        public void Create(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();        
        }
    }
}
