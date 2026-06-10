using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Usuario
{
    public class ActualizarUsuarioDTO
    {
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

        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(30)]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(250)]
        public string? Direccion { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime Nacimiento { get; set; }




    }
}
