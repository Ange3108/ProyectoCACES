using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Horario
{
    public class RegistrarHorarioDTO
    {
        [Required(ErrorMessage ="Seleccione un médico")]
        public int Id_Medico { get; set; }
        [Required(ErrorMessage ="Seleccione un día de la semana ")]
        public int DiaSemana { get; set; }
        [Required(ErrorMessage = "Establezca una hora de inicio")]
        public TimeSpan HoraInicio { get; set; }
        [Required(ErrorMessage = "Seleccione un estado")]
        public bool Activo { get; set; } = true;

    }
}
