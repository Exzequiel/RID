using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Tecnico
{
    public class ListTecnicoViewModel : EditTecnicoViewModel
    {

        public int id_tecnico { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Display(Name = "Perfil")]
        public string puesto { get; set; }

        [Display(Name = "Estado")]
        public bool? Activo { get; set; }

        //public int id_tecnico { get; set; }
        //public string nombre { get; set; }
        //public string apellido { get; set; }
        //public string puesto { get; set; }
        //public bool activo { get; set; }

    }
}