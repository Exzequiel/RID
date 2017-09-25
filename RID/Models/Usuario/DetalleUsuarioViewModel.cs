﻿using System.ComponentModel.DataAnnotations;

namespace RID.Models.Usuario
{
    public class DetalleUsuarioViewModel
    {
        public int PersonaID { get; set; }
        public string IdAspnetUser { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Activo { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public string TipoUsuario { get; set; }

        [Display(Name = "Ubicación")]
        public string NombreUbicacion { get; set; }
    }
}