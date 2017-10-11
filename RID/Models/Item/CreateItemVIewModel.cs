using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Item
{
    public class CreateItemVIewModel
    {

        [Display(Name = "Código Producto")]
        [Required]
        public string cod_item { get; set; } 

        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }

        //[Display(Name = "Ubicación")]
        //[Required]
        //public int IdUbicacion { get; set; }

        [Display(Name = "Objeto")]
        [Required]
        public int IdObjeto { get; set; }


        //[Display(Name = "Cantidad Disponible")]
        //[Required]
        //public int CantidadDisponible { get; set; }
        
    }
}