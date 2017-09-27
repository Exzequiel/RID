using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.Salida
{
    public class CrearDetalleSalidaViewModel
    {

        public string ubicacion { get; set; }
        public string objeto { get; set; }

        public string maquina { get; set; }

        public string tecnico { get; set; }

        public string descripcion { get; set; }


        public int id_detalle_salida { get; set; }
        public int id_salida { get; set; }
        public int id_item { get; set; }
        public int id_maquina { get; set; }
        public int id_tecnico { get; set; }
        public int cant_aentregar { get; set; }
        public bool activo { get; set; }
    }
}