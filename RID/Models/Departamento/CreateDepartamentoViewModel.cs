using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Departamento
{
    public class CreateDepartamentoViewModel
    {
        [Display(Name = "Departamento")]
        [Required]
        public string Description { get; set; }
    }
}