using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.SolicitudMedico
{
    public class RegistrarSolicitudFormularioDTO
    {
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(80)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(60)]
        public string PrimerApellido { get; set; } = string.Empty;

        [StringLength(60)]
        public string? SegundoApellido { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        [StringLength(120)]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(25)]
        public string Telefono { get; set; } = string.Empty;

        [Range(1, int.MaxValue,
            ErrorMessage = "Seleccione una especialidad válida.")]
        public int IdEspecialidad { get; set; }

        [Range(0, 60,
            ErrorMessage = "Los años de experiencia deben estar entre 0 y 60.")]
        public int AniosExperiencia { get; set; }

        [StringLength(500)]
        public string? Certificaciones { get; set; }

        [Required(ErrorMessage = "Debe indicar el motivo.")]
        [StringLength(500)]
        public string Motivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe adjuntar el currículum.")]
        public IFormFile Curriculum { get; set; } = null!;

        public IFormFile? Foto { get; set; }
    }
}