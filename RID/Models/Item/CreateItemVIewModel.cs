using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Item
{
    public class CreateItemVIewModel
    {

        [Display(Name = "Cód. Producto")]
        [Required]
        public string cod_item { get; set; } 

        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }

        [Display(Name = "Cód. Objeto")]
        [Required]
        public string cod_objeto { get; set; }
        
    }
}