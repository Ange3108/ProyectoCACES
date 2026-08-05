using System;
using System.Collections.Generic;
using System.Text;
namespace CACES.BLL.DTOs.SolicitudMedico
{
    public class MostrarSolicitudMedicoDTO
    {
        public int IdSolicitud { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public int IdEspecialidad { get; set; }

        public string NombreEspecialidad { get; set; } = string.Empty;

        public int AniosExperiencia { get; set; }

        public string? Certificaciones { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public string? Curriculum { get; set; }

        public string? Foto { get; set; }

        public byte Estado { get; set; }

        public string EstadoTexto { get; set; } = string.Empty;

        public string? ObservacionAdministrador { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public DateTime? FechaRespuesta { get; set; }
    }
}