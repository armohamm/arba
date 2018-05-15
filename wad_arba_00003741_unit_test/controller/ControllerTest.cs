using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wad_arba_00003741_ii.Controllers;

namespace wad_arba_00003741_unit_test.controller
{
    [TestClass]
    public class ControllerTest
    {
        private HomeController controler;
        private ProductRepo repository;


        [TestInitialize]
        public void SetUpContext()
        {
            repository = new ProductRepo();
            controler = new HomeController();
        }

        [TestMethod]
        public void CreateProduct()
        {

            //List<Product> productList = new List<Product>(repository.GetAll().ToList());
            //List<Product> products = repository.GetAll().ToList(); ;
            //int count = repository.Count();
            var p = repository.GetAll();
            int c = 0;
            //foreach (var product in products)
            //{
            //    count++;
            //}

            //Product product = new Product
            //{
            //    Category = "Smartphone",
            //    Name = "Name",
            //    Price = Convert.ToDecimal(55.99),
            //};

            //repository.Create(product);


            //Product productTest = productList[productList.Count - 1];

            //Assert.AreEqual(productTest, product);
        }

        [TestMethod]
        public void TestUpdate()
        {


            //Product product = products[0];

            //int id = product.Id;
            //product.Name = "Changes";
            //product.Category = "Changed";

            
            //repository.Update(product);

            //Product productTest = repository.GetById(id);

            //Assert.AreEqual(productTest, product);
        }
    }
}
