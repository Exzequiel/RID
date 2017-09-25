using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.TipoUsuario
{
    public class ListTipoUsuarioViewModel:CreateTipoUsuarioViewModel
    {

        public int IdTipoUsuario { get; set; }
        public bool Activo { get; set; }
    }
}