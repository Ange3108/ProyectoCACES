using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Cita
{
    public class MostrarCitaDTO
    {
        public int IdCita { get; set; }

        public int IdPaciente { get; set; }

        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
  

        public string NombrePaciente { get; set; } = string.Empty;

        public string NombreMedico { get; set; } = string.Empty;

        public string NombreEspecialidad { get; set; } = string.Empty;

        public DateTime FechaCita { get; set; }

        public TimeSpan Hora { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public byte Estado { get; set; }

        public string EstadoTexto => Estado == 1 ? "Pendiente" : "Cancelada";


    }
}