using System;
using System.ComponentModel.DataAnnotations;

namespace RID.Models.Usuario
{
    public class CrearUsuarioViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La nueva contraseña y la confirmación no son las mismas")]
        public string ConfirmPassword { get; set; }

        public string IdAspNetUser { get; set; }

        [Required]
        [Display(Name = "Tipo de Usuario")]
        public string RoleUsuario { get; set; }

        [Display(Name = "Ubicación")]
        public int IdUbicacion { get; set; }

        public bool Estado { get; set; }
    }
}