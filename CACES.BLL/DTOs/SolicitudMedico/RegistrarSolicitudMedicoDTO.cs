using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.SolicitudMedico
{
    public class RegistrarSolicitudMedicoDTO
    {
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(80)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(60)]
        public string PrimerApellido { get; set; } = string.Empty;

        [StringLength(60)]
        public string? SegundoApellido { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        [StringLength(120)]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(25)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una especialidad.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una especialidad válida.")]
        public int IdEspecialidad { get; set; }

        [Required(ErrorMessage = "Los años de experiencia son obligatorios.")]
        [Range(0, 60, ErrorMessage = "Los años de experiencia deben estar entre 0 y 60.")]
        public int AniosExperiencia { get; set; }

        [StringLength(
            500,
            ErrorMessage = "Las certificaciones no pueden superar los 500 caracteres."
        )]
        public string? Certificaciones { get; set; }

        [Required(ErrorMessage = "Debe indicar por qué desea formar parte de CACES.")]
        [StringLength(
            500,
            ErrorMessage = "El motivo no puede superar los 500 caracteres."
        )]
        public string Motivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe adjuntar el currículum.")]
        [StringLength(250)]
        public string Curriculum { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Foto { get; set; }
    }
}