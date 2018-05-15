using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Schema;
using wad_arba_00003741_ii.Models;

namespace wad_arba_00003741_ii.Controllers
{
    [Authorize]
    public class XMLServiceController : BaseController
    {
        // GET: XMLService
        public ActionResult XML(string format)
        {
            var products = new ProductRepo().GetAll();

            var xDoc = new XDocument();

            xDoc.Declaration = new XDeclaration("1.0", "utf-8", "no");

            if (format == "html")
            {
                xDoc.Add(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/xml/productToHTML.xslt'"));

            }
            else if (format == "csv")
            {
                xDoc.Add(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/xml/xmlTest.xslt'"));
            }

            xDoc.Add(new XElement("Products", products.Select(ProductToXML)));

            var schemas = new XmlSchemaSet();
            schemas.Add("","http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/xml/productSchema.xsd");

            var isValid = true;
            var errorMessage = "";
            xDoc.Validate(schemas, (o, e) =>
            {
                isValid = false;
                errorMessage = e.Message;
            }, true);

            if (!isValid)
            {
                xDoc = new XDocument();
                xDoc.Declaration = new XDeclaration("1.0", "utf-8", "no");
                xDoc.Add(new XElement("Error", errorMessage));
            }

            var sw = new StringWriter();
            xDoc.Save(sw);

            return Content(sw.ToString(), "text/xml");
        }

        private XElement ProductToXML(Product product)
        {
            XElement xmlProduct = new XElement("Product", new XAttribute("Id", product.Id),
                                                            new XElement("Name", product.Name),
                                                            new XElement("Price", product.Price),
                                                            new XElement("Category", product.Category)
                                                            );
            return xmlProduct;
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
    }
}