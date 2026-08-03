using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Cita
{
    public class EditarCitaDTO
    {
        [Required]
        public int IdCita { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        public int IdEspecialidad { get; set; }

        [Required(ErrorMessage = "El médico es obligatorio.")]
        public int IdMedico { get; set; }

        [Required(ErrorMessage = "El horario es obligatorio.")]
        public int IdHorario { get; set; }

        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaCita { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria.")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(
            100,
            ErrorMessage = "El motivo no puede superar los 100 caracteres."
        )]
        public string Motivo { get; set; } = null!;
    }
}