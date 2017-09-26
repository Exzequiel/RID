using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RID.Models.Item
{
    public class ListItemViewModel : EditItemViewModel
    {
        public bool? Activo { get; set; }
        public string Ubicacion { get; set; }
        public string Objeto { get; set; }
    }
}