using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Ubicacion
{
    public class CreateUbicacionViewModel
    {
        [Display(Name = "Ubicación")]
        [Required]
        public string Description { get; set; }
    }
}