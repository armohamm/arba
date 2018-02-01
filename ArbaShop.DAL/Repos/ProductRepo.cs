using ArbaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbaShop.DAL.Repos
{
    public class ProductRepo
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

        public void Delete(int Id)
        {
            db.Products.Remove(db.Products.Find(Id));
            db.SaveChanges();
        }

        public Product GetById(int Id)
        {
            return db.Products.Find(Id);
        }

        public void Update(Product product)
        {
            db.Products.Remove(db.Products.Find(product.Id));
            db.Products.Add(product);
            db.SaveChanges();
        }

    }
}
