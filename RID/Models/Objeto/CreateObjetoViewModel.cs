using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Objeto
{
    public class CreateObjetoViewModel
    {
        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }
        [Display(Name = "Cód. Máquina")]
        [Required]
        public string CodObjeto { get; set; }
    }
}