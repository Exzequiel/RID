using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.Departamento
{
    public class ListDepartamentoViewModel : CreateDepartamentoViewModel
    {
        public int IdDepartamento { get; set; }
        public bool? Activo { get; set; }
    }
}