//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RID.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class salida_detalle
    {
        public int id_detalle_salida { get; set; }
        public int id_salida { get; set; }
        public int it_item { get; set; }
        public int cant_aentregar { get; set; }
        public bool activo { get; set; }
    
        public virtual item item { get; set; }
        public virtual salida salida { get; set; }
    }
}
