using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RID.Models.Salida
{
    public class CrearSalidaViewModel
    {
        public int id_salida { get; set; }

        public string nro_salida { get; set; }

        [Display(Name ="Fecha de Transacción")]
        public DateTime fecha_transaccion { get; set; }

        [Required]
        [Display(Name = "Ubicación")]
        public int id_ubicacion { get; set; }

        [Required]
        [Display(Name = "Técnico")]
        public int id_tecnico { get; set; }

        [Required]
        [Display(Name = "Máquina")]
        public int id_maquina { get; set; }

        [Required]
        [Display(Name = "Objeto")]
        public int[] id_objeto { get; set; }

        public string NombreDepartamento { get; set; }

        public List<CrearDetalleSalidaViewModel> ListaDetalle { get; set; }

        public bool EsEditar { get; set; }


    }
}