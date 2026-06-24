using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class RegistrarProcedimientosDto
    {
        [Required(ErrorMessage = "Debe seleccionar un paciente.")]
        public int Id_Paciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el médico especialista.")]
        public int Id_Medico { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el procedimiento quirúrgico.")]
        public int Id_Procedimiento { get; set; }

        [Required(ErrorMessage = "La fecha y hora de la cirugía es obligatoria.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaProgramada { get; set; }
    }
}
