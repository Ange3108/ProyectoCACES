using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.SolicitudMedico
{
    public class ResponderSolicitudMedicoDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La solicitud no es válida.")]
        public int IdSolicitud { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una respuesta.")]
        [Range(3, 4, ErrorMessage = "La respuesta debe ser aprobada o rechazada.")]
        public byte Estado { get; set; }

        [StringLength(
            500,
            ErrorMessage = "La observación no puede superar los 500 caracteres."
        )]
        public string? ObservacionAdministrador { get; set; }
    }
}