using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Salida
{
    public class CrearDetalleSalidaViewModel
    {
        public string lote { get; set; }
        public string maquina { get; set; }
        public string tecnico { get; set; }
        public string descripcion { get; set; }
        public string cod_item { get; set; }
        public int id_detalle_salida { get; set; }
        public int id_salida { get; set; }
        public int id_item { get; set; }
        public int id_maquina { get; set; }
        public int id_lote { get; set; }
        public int id_tecnico { get; set; }
        public int cant_aentregar { get; set; }
        public bool activo { get; set; }
    }
}