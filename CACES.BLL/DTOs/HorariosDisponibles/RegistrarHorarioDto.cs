using CACES.BLL.DTOs.Medico;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.HorariosDisponibles
{
    public class RegistrarHorarioDto
    {
        public int IdMedico { get; set; }
        public List<DetalleHorarioDto> Horarios { get; set; }
    }

    public class DetalleHorarioDto
    {
        public int DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }

    public class AsignarHorario
    {
        public RegistrarHorarioDto Formulario { get; set; }

        public IEnumerable<MedicoDTO> MedicosDisponibles { get; set; }
    }
}
