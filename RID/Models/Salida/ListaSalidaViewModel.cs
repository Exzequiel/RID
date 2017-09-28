using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.Salida
{
    public class ListaSalidaViewModel
    {
        public int id_salida { get; set; }
        public string nro_salida { get; set; }
        public DateTime fecha_transaccion { get; set; }
        public int id_departamento { get; set; }
        public string departamento { get; set; }
        public bool confirmado { get; set; }
        public bool activo { get; set; }

    }
}