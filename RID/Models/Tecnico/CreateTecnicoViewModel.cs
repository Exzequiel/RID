using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Tecnico
{
    public class CreateTecnicoViewModel
    {
        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required]
        public string Apellido { get; set; }

        [Display(Name = "Puesto")]
        [Required]
        public string Puesto { get; set; }

    }
}