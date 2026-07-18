using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Perfil
{
    public class ActualizarPerfilDTO
    {

            [Required]
            public int IdUsuario { get; set; }
            public bool Estado { get; set; } = true;

            // --- INFORMACIÓN PERSONAL OBLIGATORIA ---
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
            public string Nombres { get; set; } = null!;

            [Required(ErrorMessage = "El primer apellido es obligatorio.")]
            [StringLength(50, ErrorMessage = "El primer apellido no puede exceder los 50 caracteres.")]
            public string PrimerApellido { get; set; } = null!;

            [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
            [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
            [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
            public string Telefono { get; set; } = null!;

            [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
            [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
            [StringLength(100, ErrorMessage = "El correo no puede exceder los 100 caracteres.")]
            public string CorreoElectronico { get; set; } = null!;

            [Required(ErrorMessage = "El DUI es obligatorio.")]
            [StringLength(10, ErrorMessage = "El DUI debe cumplir con el formato estándar.")]
            public string DUI { get; set; } = null!;

            // --- INFORMACIÓN PERSONAL OPCIONAL  ---
            [StringLength(50, ErrorMessage = "El segundo apellido no puede exceder los 50 caracteres.")]
            public string? SegundoApellido { get; set; }

            // --- SEGURIDAD COMPLETAMENTE OPCIONAL  ---
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
            public string? NuevaContraseña { get; set; }

            [DataType(DataType.Password)]
            [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden.")]
            public string? ConfirmarContraseña { get; set; }

            public bool Activar2FA { get; set; }

        // --- INFORMACIÓN DE UBICACIÓN OPCIONAL  ---
        public string Direccion { get; set; }
        public string Foto { get; set; } = null!;
    }
}

