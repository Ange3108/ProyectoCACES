using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Cita
{
    public class RegistrarCitaDTO
    {
        [Required(ErrorMessage = "El paciente es requerido")]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "El médico es requerido")]
        public int IdMedico { get; set; }

        [Required(ErrorMessage = "La especialidad es requerida")]
        public int IdEspecialidad { get; set; }

        [Required(ErrorMessage = "El horario es requerido")]
        public int IdHorario { get; set; }

        [Required(ErrorMessage = "La fecha de la cita es requerida")]
        public DateTime FechaCita { get; set; }

        [Required(ErrorMessage = "La hora es requerida")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "El motivo es requerido")]
        [StringLength(100)]
        public string Motivo { get; set; } = null!;
    }
}