﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.Lote
{
    public class ListLoteVIewModel : CreateLoteViewModel
    {
        public int IdLote { get; set; }
        public bool? Activo { get; set; }
    }
}