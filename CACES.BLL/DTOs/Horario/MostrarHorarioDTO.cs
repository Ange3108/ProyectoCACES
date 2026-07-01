using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Horario
{
    public class MostrarHorarioDTO
    {
        public int Id_Horario { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }

        public bool Activo { get; set; }


    }
}
