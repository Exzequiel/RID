﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.Salida
{
    public class ListaSalidaViewModel
    {
        //public int id_entrega { get; set; }
        //public string nro_entrega { get; set; }
        //public DateTime fecha_transaccion { get; set; }
        //public bool confirmado { get; set; }
        //public string solicitante { get; set; }
        //public int semana { get; set; }
        //public int periodo { get; set; }
        //public bool activo { get; set; }
        //public string finca { get; set; }
        //public int IdFinca { get; set; }

        public int id_salida { get; set; }
        public string nro_salida { get; set; }
        public DateTime fecha_transaccion { get; set; }
        public int id_ubicacion { get; set; }
        public string ubicacion { get; set; }
        public int id_objeto { get; set; }
        public int id_tecnico { get; set; }
        public int id_maquina { get; set; }
        public bool confirmado { get; set; }
        public bool activo { get; set; }
    }
}