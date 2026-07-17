using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class RegistrarProcedimientosDto

    {

        [Required(ErrorMessage = "Debe seleccionar un paciente.")]
        public int IdPaciente { get; set; }
        public string NombrePaciente { get; set; } = null!;
        [Required(ErrorMessage = "Debe seleccionar el médico especialista.")]
        public int IdMedico { get; set; }

        public string NombreMedico { get; set; } = null!;
        [Required(ErrorMessage = "Debe seleccionar el procedimiento quirúrgico.")]
        public int IdCirugia { get; set; }

        public string NombreCirugia { get; set; } = null!;
        public int IdCita { get; set; }

        [Required(ErrorMessage = "La fecha y hora de la cirugía es obligatoria.")]
        public string CitaFechaHora { get; set; } = null!;

    }
}
