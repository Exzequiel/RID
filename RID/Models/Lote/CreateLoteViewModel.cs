using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Lote
{
    public class CreateLoteViewModel
    {
        [Display(Name = "N° de Lote")]
        [Required]
        public string CodLote { get; set; }
    }
}