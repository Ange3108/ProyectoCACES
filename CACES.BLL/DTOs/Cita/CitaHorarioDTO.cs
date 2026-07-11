using System;
using System.Collections.Generic;
using System.Text;
namespace CACES.BLL.DTOs.Cita
{
    public class CitaHorarioDTO
    {
        public int IdHorario { get; set; }
        public int DiaSemana { get; set; }
        public string DiaTexto { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string HorarioTexto { get; set; } = string.Empty;
    }
}