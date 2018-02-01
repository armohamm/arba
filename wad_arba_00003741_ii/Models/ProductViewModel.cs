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
        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [Range(1, 999999)]
        public float Price { get; set; }

        [Required]    
        public string Category { get; set; }

        [DisplayName("Available")]
        public bool IsActive { get; set; }
    }
}