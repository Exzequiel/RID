using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Tecnico
{
    public class ListTecnicoViewModel : EditTecnicoViewModel
    {

        public bool? activo { get; set; }

    }
}