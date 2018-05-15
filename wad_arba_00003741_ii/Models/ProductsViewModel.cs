using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace wad_arba_00003741_ii.Models
{
    public class ProductsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public bool IsActive { get; set; }

        public SortCriteria Criteria { get; set; }
        public SortOrder Order { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<ProductsViewModel> Products { get; set; }
    }
}