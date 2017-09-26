using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Objeto
{
    public class CreateObjetoViewModel
    {
        [Display(Name = "Cód. Objeto")]
        [Required]
        public string CodObjeto { get; set; }
    }
}