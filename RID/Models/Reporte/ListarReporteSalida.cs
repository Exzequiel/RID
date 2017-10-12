using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RID.DB;
using RID.Models.Salida;
using System.ComponentModel.DataAnnotations;

namespace RID.Models.Reporte
{
    public class ListarReporteSalida 
    {
        [Display(Name = "Cód. Ítem")]
        public string cod_item { get; set; }
        [Display(Name = "Unidades")]
        public int cant_aentregar { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
        [Display(Name = "Cód. Objeto")]
        public string cod_objeto { get; set; }
        [Display(Name = "Máquina")]
        public string maquina { get; set; }
        [Display(Name = "Lote")]
        public string lote { get; set; }

        public List<ListarReporteSalida> ListaReporte { get; set; }

    }
 }
