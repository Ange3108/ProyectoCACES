using System;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Usuario
{
    public class RegistrarUsuarioDTO
    {
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El primer apellido es requerido")]
        [StringLength(100)]
        public string PrimerApellido { get; set; } = null!;

        [Required(ErrorMessage = "El segundo apellido es requerido")]
        [StringLength(100)]
        public string SegundoApellido { get; set; } = null!;

        [Required(ErrorMessage = "El correo es requerido")]
        [StringLength(200)]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        public string CorreoElectronico { get; set; } = null!;

        [Required(ErrorMessage = "El DUI es requerido")]
        [StringLength(10)]
        public string DUI { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(30)]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; } = null!;
        [StringLength(250)]
        public string? Direccion { get; set; }
        [Required(ErrorMessage = "Ingrese su fecha de nacimiento")]
        public DateTime Nacimiento { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 8)]
        public string passwordHash { get; set; } = null!;



    }
}