using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wad_arba_00003741_ii.Models
{
    public class ProductImportModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; } 
    }
}