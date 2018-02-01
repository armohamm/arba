using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace wad_arba_00003741_ii.Models
{
    public class ProductsViewModel
    {
        public string Category { get; set; }
        public String Name { get; set; }
        public SortCriteria Criteria { get; set; }
        public SortOrder Order { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}