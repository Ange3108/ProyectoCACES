using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Medico
{
    public class EditarMedicoDTO
    {
        public int IdMedico { get; set; }
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string PrimerApellido { get; set; } = null!;

        [Required(ErrorMessage = "El segundo apellido es obligatorio")]
        public string SegundoApellido { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; } = null!;

        public string? Foto { get; set; }

        public bool Estado { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        public int IdEspecialidad { get; set; }

        [Required(ErrorMessage = "La experiencia es obligatoria")]
        public int Experiencia { get; set; }

        [Required(ErrorMessage = "Las certificaciones son obligatorias")]
        public string Certificaciones { get; set; } = null!;
    }
}