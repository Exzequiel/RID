using System;
using System.ComponentModel.DataAnnotations;

namespace RID.Models.Usuario
{
    public class ListaUsuarioViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Perfil")]
        public string Perfil { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        [Display(Name = "Departamentos")]
        public string NombreDepartamento { get; set; }
    }
}