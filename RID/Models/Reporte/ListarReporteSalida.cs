using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RID.DB;
using RID.Models.Salida;

namespace RID.Models.Reporte
{
    public class ListarReporteSalida 
    {

        public string cod_item { get; set; }
        public int cant_aentregar { get; set; }
        public string descripcion { get; set; }
        public string cod_objeto { get; set; }
        public string maquina { get; set; }
        public string lote { get; set; }

        public List<ListarReporteSalida> ListaReporte { get; set; }

    }
 }
