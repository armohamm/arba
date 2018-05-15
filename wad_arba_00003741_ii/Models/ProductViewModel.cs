using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wad_arba_00003741_ii.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [Range(1, 999999)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(2)]
        public string Category { get; set; }

        [DisplayName("Available")]
        public bool IsActive { get; set; }
       
    }

    //public enum Category
    //{
    //    Smartphone,
    //    Tablets
    //}
}