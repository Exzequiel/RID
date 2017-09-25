using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.Ubicacion
{
    public class ListUbicacionVIewModel : CreateUbicacionViewModel
    {
        public int IdUbicacion { get; set; }
        public bool? Activo { get; set; }
    }
}