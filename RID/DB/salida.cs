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
    
    public partial class salida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public salida()
        {
            this.salida_detalle = new HashSet<salida_detalle>();
        }
    
        public int id_salida { get; set; }
        public string nro_salida { get; set; }
        public System.DateTime fecha_transaccion { get; set; }
        public int id_ubicacion { get; set; }
        public int id_objeto { get; set; }
        public int id_tecnico { get; set; }
        public bool activo { get; set; }
    
        public virtual objeto objeto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<salida_detalle> salida_detalle { get; set; }
        public virtual tecnico tecnico { get; set; }
        public virtual ubicacion ubicacion { get; set; }
    }
}